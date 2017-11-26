using Newtonsoft.Json;
using System;
using System.Threading;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareClient : MinerMiddleware
  {
    protected override bool isServer
    {
      get
      {
        return false;
      }
    }

    readonly XmrDll dll = new XmrDll();

    public MiddlewareClient()
    {
      onMessage += MiddlewareClient_onMessage;
    }

    void MiddlewareClient_onMessage(
      IMessage message)
    {
      if (message is StartMiningRequest startMiningRequest)
      {
        string config = Xmr.GenerateConfigJson(
          startMiningRequest.wallet,
          startMiningRequest.numberOfThreads,
          startMiningRequest.workerName);
        dll.StartMining(config);
      }
      else if (message is SetSleepFor sleepFor)
      {
        dll.SetSleepFor(sleepFor.sleepForInNanoseconds);
      }
    }

    public void Run()
    {
      while (true)
      {
        Send(new MiningStats("BB", hashRate: dll.totalHashRate, acceptedHashRate: 0));
        Thread.Sleep(3000);
      }
    }
  }
}
