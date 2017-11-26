using System;

namespace HD
{
  public class XmrMiddlewareClient : MiddlewareClient
  {
    readonly XmrDll dll = new XmrDll();

    public override double hashRate
    {
      get
      {
        return dll.totalHashRate;
      }
    }

    public override string algorithmName
    {
      get
      {
        return "CryptoNight";
      }
    }

    public override void OnMessage(
      StartMiningRequest startMiningRequest)
    {
      string config = Xmr.GenerateConfigJson(
          startMiningRequest.wallet,
          startMiningRequest.numberOfThreads,
          startMiningRequest.workerName);
      dll.StartMining(config);
    }

    public override void OnMessage(SetSleepFor sleepFor)
    {
      dll.SetSleepFor(sleepFor.sleepForInNanoseconds);
    }
  }
}
