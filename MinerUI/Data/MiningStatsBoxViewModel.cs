using System;
using System.Diagnostics;
using System.Windows;

namespace HD
{
  public class MiningStatsBoxViewModel : StatsBoxViewModel
  {
    #region Data
    StatsBoxUseCase useCase;
    #endregion

    #region Properties
    public double btcAmount
    {
      set
      {
        if (value > 0)
        {
          LeftValue = $"{value:N9} BTC";
          double dollarAmount = value * Miner.instance.settings.bitcoinPrice.dollarPerBitcoin;
          RightValue = $"${dollarAmount:N4}";
          RightValueVisibility = Visibility.Visible;
        }
        else
        {
          LeftValue = "No data";
          RightValueVisibility = Visibility.Collapsed;
        }
      }
    }

    public double dailyEstimatedBtc
    {
      get
      {
        return
          Miner.instance.currentMiner.currentHashRateMHpS
          * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH;
      }
    }

    string intervalString
    {
      get
      {
        // TODO
        return "Monthly";
      }
    }

    double daysPerInterval
    {
      get
      {
        return 30;
      }
    }
    #endregion

    #region Init
    public MiningStatsBoxViewModel(
      bool isPrimary,
      StatsBoxUseCase useCase)
    {
      this.useCase = useCase;
      Refresh();

      switch (useCase)
      {
        case StatsBoxUseCase.IntervalEstimatedEarningsFromMe:
          this.Title = $"{intervalString} estimated earnings from you";
          break;
        case StatsBoxUseCase.IntervalEstimatedEarningsFromAllCurrentMiners:
          this.Title = $"{intervalString} estimated earnings for all contributors";
          break;
        case StatsBoxUseCase.TotalContribution:
          this.Title = "Estimated Total Earnings";
          break;
        default:
          Debug.Fail("Missing use case");
          break;
      }

      if (isPrimary)
      {
        BackgroundColor = "#FF00FF00";
      }
      else
      {
        BackgroundColor = "#FF555555";
      }
    }
    #endregion

    #region Public
    public void Refresh()
    {
      switch (useCase)
      {
        case StatsBoxUseCase.IntervalEstimatedEarningsFromMe:
          if (Miner.instance.currentMiner == null)
          {
            this.btcAmount = 0;
          }
          else
          {
            this.btcAmount = (Miner.instance.currentMiner.currentHashRateMHpS * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH) * daysPerInterval;
          }
          break;
        case StatsBoxUseCase.IntervalEstimatedEarningsFromAllCurrentMiners:
          this.btcAmount = (Miner.instance.settings.beneficiaries.totalWorkerHashRateMHpS * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH) * daysPerInterval;
          break;
        case StatsBoxUseCase.TotalContribution:
          this.btcAmount = Miner.instance.settings.beneficiaries.totalContributionInBitcoin;
          break;
        default:
          Debug.Fail("Missing use case");
          break;
      }
    }
    #endregion
  }
}
