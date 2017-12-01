using System;

namespace HD
{
  public class CurrentMiningStatsViewModel : MiningStatsBoxViewModel
  {
    public override MinerPeformance currentMiningPerformance
    {
      get
      {
        return new MinerPeformance(beneficiary.localHashRate 
          * Miner.instance.pricePerDayInBtcFor1MHOfCryptonight);
      }
    }

    protected override Beneficiary beneficiary
    {
      get
      {
        return Miner.instance.currentWinner;
      }
    }

    public CurrentMiningStatsViewModel(
      MainViewModel mainViewModel)
      : base(mainViewModel)
    {
      MiddlewareServer.onMiningStatsUpdate += MiddlewareServer_onMiningStatsUpdate;
    }

    void MiddlewareServer_onMiningStatsUpdate(
      MiningStats stats)
    {
      Debug.Assert(stats != null);

      OnPropertyChanged(nameof(currentMiningPerformance));
      OnPropertyChanged(nameof(currentMiningPerformanceString));
    }
  }
}
