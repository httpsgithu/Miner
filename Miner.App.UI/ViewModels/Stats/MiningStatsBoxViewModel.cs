using HD.Controllers;
using System;
using System.Diagnostics;

namespace HD
{
  /// <summary>
  /// Base view model for a wallet's current performance.
  /// </summary>
  public abstract class MiningStatsBoxViewModel : ViewModel
  {
    #region Data
    readonly MainViewModel mainViewModel;
    #endregion

    #region Properties
    /// <summary>
    /// True if I might mine for this wallet
    /// </summary>
    public bool isActive
    {
      get
      {
        return beneficiary?.isValidAndActive ?? false; 
      }
    }

    public abstract MinerPeformance currentMiningPerformance
    {
      get;
    }

    public MinerPeformance poolsMiningPerformance
    {
      get
      {
        if(beneficiary == null)
        {
          return default(MinerPeformance);
        }

        return new MinerPeformance(beneficiary.totalWorkerHashRateMHpS
          * Miner.instance.pricePerDayInBtcFor1MHOfCryptonight);
      }
    }

    /// <summary>
    /// Represents either my work or the total pools.
    /// 
    /// $3.12 or .0001 BTC.
    /// Considers currency and interval settings.
    /// </summary>
    public string currentMiningPerformanceString
    {
      get
      {
        if (mainViewModel.shouldShowBtcVsD)
        {
          return currentMiningPerformance.btcString;
        }
        else
        {
          return currentMiningPerformance.currencyString;
        }
      }
    }

    public string wallet
    {
      get
      {
        return beneficiary?.wallet;
      }
    }

    public string walletOwner
    {
      get
      {
        return beneficiary?.name;
      }
    }

    public double percentSupport
    {
      get
      {
        return beneficiary?.percentTime ?? 0;
      }
    }

    protected abstract Beneficiary beneficiary { get; }
    #endregion

    #region Init
    public MiningStatsBoxViewModel(
      MainViewModel mainViewModel)
    {
      Debug.Assert(mainViewModel != null);

      this.mainViewModel = mainViewModel;
      MiddlewareServer.onMiningStatsUpdate += MiddlewareServer_onMiningStatsUpdate;
      Miner.instance.settings.minerConfig.onCurrencyChange += Config_OnCurrencyUpdated;
    }
    #endregion

    #region Events
    void MiddlewareServer_onMiningStatsUpdate(
      MiningStats stats)
    {
      Debug.Assert(stats != null);

      OnPropertyChanged(nameof(currentMiningPerformance));
      OnPropertyChanged(nameof(currentMiningPerformanceString));
    }

    void Config_OnCurrencyUpdated(Currency currency)
    {
      OnPropertyChanged(nameof(currentMiningPerformanceString));
    }
    #endregion
  }
}
