using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;

namespace HD
{
  /// <summary>
  /// Selects the stratum URL to use by pinging each region.  
  /// When a region starts failing to provide work we will select the next best ping.
  /// </summary>
  /// <remarks>
  /// This logic could be extended overtime to include multiple pools potentially 
  /// (but I worry about ever hitting mins)
  /// </remarks>
  public class MinerRegionMonitor
  {
    public class Region
    {
      public readonly string url;
      public readonly int port;

      /// <summary>
      /// negative is unknown
      /// long.maxvalue is no reply
      /// else the roundtrip time
      /// </summary>
      public long ping;
      public DateTime? timeStallStartedIfCurrentlyStalled;

      public Region(
        string url,
        int port)
      {
        this.url = url;
        this.port = port;
        this.ping = -1;
        this.timeStallStartedIfCurrentlyStalled = null;
      }

      public string stratumUrl
      {
        get
        {
          return $"{url}:{port}";
        }
      }
    }

    public class RegionSorter : IComparer<Region>
    {
      /// <summary>
      /// -1 means x less than y
      /// 0 means x == y
      /// 1 means x greater than y
      /// </summary>
      int IComparer<Region>.Compare(
        Region x,
        Region y)
      {
        if (x.ping < 0)
        {
          return -1;
        }

        if (y.ping < 0)
        {
          return 1;
        }

        return x.ping.CompareTo(y.ping);
      }
    }

    readonly List<Region> regionList = new List<Region>();
    int currentRegionIndex;

    readonly Timer pingRegionsTimer = new Timer(1);

    public Region currentRegion
    {
      get
      {
        return regionList[currentRegionIndex];
      }
    }

    public MinerRegionMonitor(
      MiddlewareServer middlewareServer)
    {
      regionList.Add(new Region("cryptonight.usa.nicehash.com", 3355));
      regionList.Add(new Region("cryptonight.eu.nicehash.com", 3355));
      regionList.Add(new Region("cryptonight.hk.nicehash.com", 3355));
      regionList.Add(new Region("cryptonight.jp.nicehash.com", 3355));
      regionList.Add(new Region("cryptonight.in.nicehash.com", 3355));
      regionList.Add(new Region("cryptonight.br.nicehash.com", 3355));

      pingRegionsTimer.AutoReset = false;
      pingRegionsTimer.Elapsed += PingRegionsTimer_Elapsed;
      pingRegionsTimer.Start();

      middlewareServer.onWorkIsStalled += MiddlewareServer_onWorkIsStalled;
    }

    void PingRegionsTimer_Elapsed(
      object sender,
      ElapsedEventArgs e)
    {
      pingRegionsTimer.Interval = TimeSpan.FromHours(2).TotalMilliseconds;

      Region currentRegion = this.currentRegion;

      for (int i = 0; i < regionList.Count; i++)
      {
        Region region = regionList[i];
        Ping ping = new Ping();
        PingReply reply = ping.Send(region.url, 3000);
        if (reply.Status != IPStatus.Success)
        {
          region.ping = long.MaxValue;
        }
        else
        {
          region.ping = reply.RoundtripTime;
        }
      }

      regionList.Sort(new RegionSorter());
      currentRegionIndex = 0;

      Debug.Assert(regionList[0].ping <= regionList[1].ping);
      Debug.Assert(regionList[1].ping <= regionList[2].ping);
      Debug.Assert(regionList[2].ping <= regionList[3].ping);
      Debug.Assert(regionList[3].ping <= regionList[4].ping);

      if (this.currentRegion != currentRegion)
      { // A region with a better ping was found, consider a reconnect
        Log.Info($"Ping changed the preferred region to {this.currentRegion} from {currentRegion}");
        Miner.instance.RestartIfRunning();
      }

      pingRegionsTimer.Start();
    }

    void MiddlewareServer_onWorkIsStalled(
      WorkIsStalled stalledMessage)
    {
      if (currentRegion.timeStallStartedIfCurrentlyStalled == null
        && stalledMessage.isStalled)
      {
        currentRegion.timeStallStartedIfCurrentlyStalled = DateTime.Now;
      }
      else if (stalledMessage.isStalled == false)
      {
        currentRegion.timeStallStartedIfCurrentlyStalled = null;
        return;
      }

      Debug.Assert(currentRegion.timeStallStartedIfCurrentlyStalled != null);

      TimeSpan howLongWeHaveBeenStalled = DateTime.Now - currentRegion.timeStallStartedIfCurrentlyStalled.Value;

      if (howLongWeHaveBeenStalled > TimeSpan.FromSeconds(20))
      { // We have been stalled for a bit, let's bail.
        Region previousRegion = currentRegion;
        currentRegionIndex++;
        if (currentRegionIndex >= regionList.Count)
        {
          currentRegionIndex = 0;
        }
        Log.Info($"Pool stalled caused the preferred region to change to {this.currentRegion} from {previousRegion}");
        Miner.instance.RestartIfRunning();
      }
    }
  }
}
