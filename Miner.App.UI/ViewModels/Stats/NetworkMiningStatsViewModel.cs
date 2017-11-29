using System;

namespace HD
{
  public class NetworkMiningStatsViewModel : MiningStatsBoxViewModel
  {
    readonly Beneficiary _beneficiary;
    
    protected override Beneficiary beneficiary
    {
      get
      {
        return _beneficiary;
      }
    }

    public override MoneyValue currentMiningPerformance
    {
      get
      {
        return poolsMiningPerformance;
      }
    }

    public NetworkMiningStatsViewModel(
      MainViewModel mainViewModel,
      Beneficiary beneficiary)
      : base(mainViewModel)
    {
      Debug.Assert(beneficiary != null);

      this._beneficiary = beneficiary;
    }
  }
}
