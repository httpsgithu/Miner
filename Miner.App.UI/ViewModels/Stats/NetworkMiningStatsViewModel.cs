using System;

namespace HD.ViewModels
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

    public NetworkMiningStatsViewModel(
      Beneficiary beneficiary)
    {
      this._beneficiary = beneficiary;
    }
  }
}
