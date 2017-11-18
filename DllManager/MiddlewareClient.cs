using Newtonsoft.Json;
using System;

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
      if(message is StartMiningRequest startMiningRequest)
      {
        string config = Xmr.GenerateConfigJson(
          startMiningRequest.wallet,
          startMiningRequest.numberOfThreads,
          startMiningRequest.workerName);
        dll.StartMining(config);
      }
    }
  }
}
