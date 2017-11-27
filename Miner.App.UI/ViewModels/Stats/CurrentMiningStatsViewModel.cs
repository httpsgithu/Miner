using System;

namespace HD.ViewModels
{
  public class CurrentMiningStatsViewModel : MiningStatsBoxViewModel
  {
    protected override Beneficiary beneficiary
    {
      get
      {
        return Miner.instance.currentWinner;
      }
    }
  }
}
