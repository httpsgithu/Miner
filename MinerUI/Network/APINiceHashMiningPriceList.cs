using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HD
{
  public class APINiceHashMiningPriceList : NetworkAPI
  {
    #region JSON
    public class MiningPriceList
    {
      public string Method { get; set; }
      public Result Result { get; set; }
    }

    public class Result
    {
      public Stat[] Stats { get; set; }
    }

    public class Stat
    {
      public string ProfitabilityAboveEth { get; set; }
      public string Price { get; set; }
      public long Algo { get; set; }
      public string ProfitabilityAboveBtc { get; set; }
      public string ProfitabilityBtc { get; set; }
      public string ProfitabilityLtc { get; set; }
      public string ProfitabilityAboveLtc { get; set; }
      public string ProfitabilityEth { get; set; }
      public string Speed { get; set; }
    }
    #endregion

    public double pricePerDayInBtcFor1MH;

    static readonly Uri niceHashGlobalStatsUri = new Uri("https://api.nicehash.com/api?method=stats.global.current");

    public APINiceHashMiningPriceList()
      : base(niceHashGlobalStatsUri, TimeSpan.FromMinutes(10))
    { }

    protected override void OnDownloadComplete(
      string content)
    {
      try
      {
        MiningPriceList priceList = JsonConvert.DeserializeObject<MiningPriceList>(content);
        pricePerDayInBtcFor1MH = 0;

        for (int i = 0; i < priceList.Result.Stats.Length; i++)
        {
          Stat stat = priceList.Result.Stats[i];
          if (stat.Algo == 22)
          {
            pricePerDayInBtcFor1MH = double.Parse(stat.Price, NumberStyles.Any, CultureInfo.InvariantCulture);
            break;
          }
        }
      }
      catch (Exception e)
      {
        Log.NetworkError(nameof(APINiceHashMiningPriceList), nameof(OnDownloadComplete), e);
      }
    }

    public override void BeginRead(
      bool skipCooldownCheck = false)
    {
      base.BeginRead(skipCooldownCheck || pricePerDayInBtcFor1MH <= 0);
    }
  }
}
