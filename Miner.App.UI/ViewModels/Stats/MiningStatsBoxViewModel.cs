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
        return beneficiary.isValidAndActive;
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
        return new MinerPeformance(new decimal(beneficiary.totalWorkerHashRateMHpS)
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
          return currentMiningPerformance.usdString;
        }
      }
    }

    public string wallet
    {
      get
      {
        return beneficiary.wallet;
      }
    }

    public string walletOwner
    {
      get
      {
        return beneficiary.name;
      }
    }

    public double percentSupport
    {
      get
      {
        return beneficiary.percentTime;
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
    }
    #endregion
  }
}
