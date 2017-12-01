using HD.Controllers;
using System;

namespace HD
{
  public struct MinerPeformance
  {
    readonly decimal btcPerDay;

    decimal usdDay
    {
      get
      {
        // TODO remove
        Debug.Assert(Math.Abs(btcPerDay) < 1);
        Debug.Assert(Miner.instance.dollarPerBitcoin < 12000);

        return btcPerDay * Miner.instance.dollarPerBitcoin;
      }
    }

    public string btcString
    {
      get
      {
        if(btcPerDay < 0)
        {
          return "loading...";
        }

        return $"{Miner.instance.settings.minerConfig.period.DailyToPeriod(btcPerDay):N8} BTC";
      }
    }

    public string currencyString
    {
      get
      {
        if(usdDay < 0)
        {
          return "loading...";
        }
        var value = usdDay;
        if(Miner.instance.SelectedCurrency != "USD")
            CurrencyExchangeManager.From(usdDay, "USD").To(Miner.instance.SelectedCurrency);
        return $"{Miner.instance.settings.minerConfig.period.DailyToPeriod(value):N2} {Miner.instance.SelectedCurrency}";
      }
    }
    
    public MinerPeformance(
      decimal btcPerDay)
    {
      this.btcPerDay = btcPerDay;
    }
  }
}
