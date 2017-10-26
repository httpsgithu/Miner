using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HD
{
  public class APIBitcoinPrice : NetworkAPI
  {
    #region JSON
    public class BitcoinPrice
    {
      public string ChartName { get; set; }
      public Bpi Bpi { get; set; }
      public string Disclaimer { get; set; }
      public Time Time { get; set; }
    }

    public class Bpi
    {
      public EUR GBP { get; set; }
      public EUR EUR { get; set; }
      public EUR USD { get; set; }
    }

    public class EUR
    {
      public string Description { get; set; }
      public double RateFloat { get; set; }
      public string Code { get; set; }
      public string Rate { get; set; }
      public string Symbol { get; set; }
    }

    public class Time
    {
      public string UpdatedISO { get; set; }
      public string Updated { get; set; }
      public string Updateduk { get; set; }
    }
    #endregion

    public double dollarPerBitcoin;

    static readonly Uri currentPriceUri = new Uri("https://api.coindesk.com/v1/bpi/currentprice.json");

    public APIBitcoinPrice()
      : base(currentPriceUri, TimeSpan.FromMinutes(30))
    { }

    protected override void OnDownloadComplete(
      string content)
    {
      try
      {
        BitcoinPrice price = JsonConvert.DeserializeObject<BitcoinPrice>(content);
        dollarPerBitcoin = double.Parse(price.Bpi.USD.Rate);
      }
      catch (Exception e)
      {
        Log.ParsingError(nameof(APIBitcoinPrice), nameof(OnDownloadComplete), e);
      }
    }

    public override void BeginRead(
      bool skipCooldownCheck = false)
    {
      base.BeginRead(skipCooldownCheck || dollarPerBitcoin <= 0);
    }
  }
}

