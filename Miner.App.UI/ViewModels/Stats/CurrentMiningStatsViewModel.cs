using System;

namespace HD
{
  public class CurrentMiningStatsViewModel : MiningStatsBoxViewModel
  {
    public override MinerPeformance currentMiningPerformance
    {
      get
      {
        if(beneficiary == null)
        {
          return default(MinerPeformance);
        }

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
      : base(mainViewModel) { }
  }
}
