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
        if(Miner.instance.settings.minerConfig.currency != Currencies.USD)
            value = CurrencyExchangeManager.From(usdDay, Currencies.USD).To(Miner.instance.settings.minerConfig.currency);

        return $"{Miner.instance.settings.minerConfig.period.DailyToPeriod(value):N2} {Miner.instance.settings.minerConfig.currency.ToString()}";
      }
    }
    
    public MinerPeformance(
      decimal btcPerDay)
    {
      this.btcPerDay = btcPerDay;
    }
  }
}
