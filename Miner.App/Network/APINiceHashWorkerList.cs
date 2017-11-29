using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace HD
{
  public class APINiceHashWorkerList : NetworkAPI
  {
    #region JSON
    public class WorkerList
    {
      public string Method { get; set; }
      public Result Result { get; set; }
    }

    public class Result
    {
      public long Algo { get; set; }
      public string Addr { get; set; }
      public object[][] Workers { get; set; }
    }
    #endregion

    #region Constants
    const string urlParam0Wallet = "https://api.nicehash.com/api?method=stats.provider.workers&addr={0}&algo=22";
    #endregion

    #region Data
    public double totalWorkerHashRateMHpS = -1;
    #endregion

    #region Init
    public APINiceHashWorkerList(
      string wallet)
      : base(new Uri(string.Format(urlParam0Wallet, wallet)))
    { }
    #endregion

    #region Events
    protected override void OnDownloadComplete(
      string content)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(content) == false);

      WorkerList data = JsonConvert.DeserializeObject<WorkerList>(content);
      Debug.Assert(data != null);

      if(data.Result.Workers == null || data.Result.Workers.Length == 0)
      { // No data ATM
        totalWorkerHashRateMHpS = 0;
        return;
      } 

      double totalSpeed = 0;
      int dataCount = 0;
      try
      {
        foreach (object[] worker in data.Result.Workers)
        {
          Dictionary<string, string> speedObject = ((JObject)worker[1]).ToObject<Dictionary<string, string>>();
          speedObject.TryGetValue("a", out string speedString);
          if (string.IsNullOrEmpty(speedString) == false)
          {
            double speed;
            double.TryParse(speedString, out speed);
            if (speed > 0)
            {
              totalSpeed += speed;
              dataCount++;
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.NetworkError(nameof(APINiceHashWorkerList), nameof(OnDownloadComplete), e);
      }

      double averageSpeed = totalSpeed / dataCount;
      totalSpeed += averageSpeed * (data.Result.Workers.Length - dataCount);

      totalWorkerHashRateMHpS = totalSpeed / 1000;

      Miner.instance.OnStatsChange();
    }
    #endregion
  }
}
