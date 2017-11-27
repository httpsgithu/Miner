using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

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

    #region Data
    public double dollarPerBitcoin;

    static readonly Uri currentPriceUri = new Uri("https://api.coindesk.com/v1/bpi/currentprice.json");
    #endregion

    public APIBitcoinPrice()
      : base(currentPriceUri, TimeSpan.FromMinutes(30))
    { }

    protected override void OnDownloadComplete(
      string content)
    {
      try
      {
        BitcoinPrice price = JsonConvert.DeserializeObject<BitcoinPrice>(content);
        dollarPerBitcoin = double.Parse(price.Bpi.USD.Rate, NumberStyles.Any, CultureInfo.InvariantCulture);
      }
      catch (Exception e)
      {
        Log.ParsingError(nameof(APIBitcoinPrice), nameof(OnDownloadComplete), e);
      }
    }

    public override void ReadWhenReady(
      bool skipCooldownCheck = false)
    {
      base.ReadWhenReady(skipCooldownCheck || dollarPerBitcoin <= 0);
    }
  }
}

