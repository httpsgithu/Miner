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
        if (btcPerDay < 0)
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
        var value = usdDay;
        if (Miner.instance.settings.minerConfig.currency != Currency.USD)
          value = CurrencyExchangeManager.From(usdDay, Currency.USD).To(Miner.instance.settings.minerConfig.currency);

        if (value < 0)
          return "loading...";

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
