using System;
using System.Diagnostics;

namespace HD
{
  public abstract class MiningStatsBoxViewModel : ViewModel
  {
    #region Data
    #endregion

    #region Properties
    protected abstract Beneficiary beneficiary { get; }

    /// <summary>
    /// True if miners are currently generating > $0 ATM.
    /// </summary>
    public bool isActive
    {
      get
      {
        // TODO beneficiary.
        return true;
      }
    }

    /// <summary>
    /// $3.12 or .0001 BTC.
    /// Considers currency and interval settings.
    /// </summary>
    public string currentEstValue
    {
      get
      {
        //if (value > 0)
        //{
        //  LeftValue = $"{value:N9} BTC";
        //  double dollarAmount = value * Miner.instance.settings.bitcoinPrice.dollarPerBitcoin;
        //  RightValue = $"${dollarAmount:N4}";
        //  //RightValueVisibility = Visibility.Visible;
        //}
        //else
        //{
        //  LeftValue = "No data";
        //  //RightValueVisibility = Visibility.Collapsed;
        //}

        return "TODO";
      }
    }

    public string walletOwner
    {
      get
      {
        return beneficiary.name;
      }
    }
    #endregion
  }
}
