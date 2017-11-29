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
        return $"{btc:N8} BTC";
      }
    }

    public string usdString
    {
      get
      {
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
