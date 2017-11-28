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

    #region Constants
    static readonly Uri currentPriceUri = new Uri("https://api.coindesk.com/v1/bpi/currentprice.json");
    #endregion

    #region Data
    public double dollarPerBitcoin;
    #endregion

    #region Init
    public APIBitcoinPrice()
      : base(currentPriceUri)
    { }
    #endregion

    #region Events
    protected override void OnDownloadComplete(
      string content)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(content) == false);

      try
      {
        BitcoinPrice price = JsonConvert.DeserializeObject<BitcoinPrice>(content);
        Debug.Assert(price != null);

        dollarPerBitcoin = double.Parse(price.Bpi.USD.Rate, NumberStyles.Any, CultureInfo.InvariantCulture);
        Miner.instance.OnStatsChange();
      }
      catch (Exception e)
      {
        Log.ParsingError(nameof(APIBitcoinPrice), nameof(OnDownloadComplete), e);
      }
    }
    #endregion

    #region API
    /// <summary>
    /// Skips the cooldown if we have not gotten any data yet.
    /// </summary>
    public override void ReadWhenReady(
      bool skipCooldownCheck = false)
    {
      base.ReadWhenReady(skipCooldownCheck || dollarPerBitcoin <= 0);
    }
    #endregion
  }
}

