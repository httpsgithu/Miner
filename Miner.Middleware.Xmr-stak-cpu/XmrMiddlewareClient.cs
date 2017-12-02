using System;

namespace HD
{
  public class XmrMiddlewareClient : MiddlewareClient
  {
    #region Data
    readonly Xmr dll = new Xmr();
    #endregion

    #region Properties
    public override double hashRate
    {
      get
      {
        double rate = dll.totalHashRate;
        Debug.Assert(rate >= 0);

        return rate;
      }
    }

    public override string algorithmName
    {
      get
      {
        return "CryptoNight";
      }
    }
    #endregion

    #region Events
    public override void OnMessage(
      StartMiningRequest startMiningRequest)
    {
      Debug.Assert(startMiningRequest != null);
      Debug.Assert(startMiningRequest.numberOfThreads > 0);

      dll.StartMining(startMiningRequest.wallet,
          startMiningRequest.numberOfThreads,
          startMiningRequest.workerName,
          startMiningRequest.stratumUrl);
    }

    public override void OnMessage(
      SetSleepFor sleepFor)
    {
      Debug.Assert(sleepFor != null);

      bool isWorkStalled = dll.SetSleepFor(sleepFor.sleepRate);

      Send(new WorkIsStalled(isWorkStalled));
    }
    #endregion
  }
}
