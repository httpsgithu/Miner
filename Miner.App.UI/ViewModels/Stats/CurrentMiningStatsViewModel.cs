using System;

namespace HD
{
  public class CurrentMiningStatsViewModel : MiningStatsBoxViewModel
  {
    double hashRate = -1;

    public override MoneyValue currentMiningPerformance
    {
      get
      {
        decimal value = new decimal(hashRate);
        return new MoneyValue(value * Miner.instance.pricePerDayInBtcFor1MHOfCryptonight);
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
      Debug.Assert(mainViewModel != null);

      Miner.instance.middlewareServer.onMiningStatsUpdate += MiddlewareServer_onMiningStatsUpdate;
    }

    void MiddlewareServer_onMiningStatsUpdate(
      MiningStats stats)
    {
      Debug.Assert(stats != null);

      this.hashRate = stats.hashRate;
      OnPropertyChanged(nameof(currentMiningPerformance));
      OnPropertyChanged(nameof(currentMiningPerformanceString));
    }
  }
}
