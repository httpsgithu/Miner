using System;

namespace HD
{
  public struct MoneyValue
  {
    public readonly decimal btc;

    public decimal usd
    {
      get
      {
        // TODO remove
        Debug.Assert(Math.Abs(btc) < 1);
        Debug.Assert(Miner.instance.dollarPerBitcoin < 12000);

        return btc * Miner.instance.dollarPerBitcoin;
      }
    }

    public string btcString
    {
      get
      {
        if(btc < 0)
        {
          return "loading...";
        }

        return $"{btc:N8} BTC";
      }
    }

    public string usdString
    {
      get
      {
        if(usd < 0)
        {
          return "loading...";
        }

        return $"{usd:C2}";
      }
    }
    
    public MoneyValue(
      decimal btc)
    {
      this.btc = btc;
    }
  }
}
