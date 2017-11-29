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
