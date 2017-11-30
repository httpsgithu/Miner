using Newtonsoft.Json;
using System;
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

    #region Constants
    static readonly Uri niceHashGlobalStatsUri = new Uri("https://api.nicehash.com/api?method=stats.global.current");
    #endregion

    #region Data
    public double pricePerDayInBtcFor1MHOfCryptonight;
    #endregion

    #region Init
    public APINiceHashMiningPriceList()
      : base(niceHashGlobalStatsUri)
    { }
    #endregion

    #region Events
    protected override void OnDownloadComplete(
      string content)
    {
      try
      {
        MiningPriceList priceList = JsonConvert.DeserializeObject<MiningPriceList>(content);

        for (int i = 0; i < priceList.Result.Stats.Length; i++)
        {
          Stat stat = priceList.Result.Stats[i];
          if (stat.Algo == 22)
          {
            pricePerDayInBtcFor1MHOfCryptonight = double.Parse(stat.Price, NumberStyles.Any, CultureInfo.InvariantCulture);
            Debug.Assert(pricePerDayInBtcFor1MHOfCryptonight > 0);

            Miner.instance.OnStatsChange();
            return;
          }
        }
      }
      catch (Exception e)
      {
        Log.NetworkError(nameof(APINiceHashMiningPriceList), nameof(OnDownloadComplete), e);
      }

      Log.Error("Missing algorithm in the coindesk API call");
    }
    #endregion

    #region Public API
    /// <summary>
    /// Skip the cooldown if we have not read any data yet.
    /// </summary>
    public override void ReadWhenReady(
      bool skipCooldownCheck = false)
    {
      base.ReadWhenReady(skipCooldownCheck || pricePerDayInBtcFor1MHOfCryptonight <= 0);
    }
    #endregion
  }
}
