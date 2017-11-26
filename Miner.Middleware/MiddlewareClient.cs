using System.Threading;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public abstract class MiddlewareClient : MinerMiddleware
  {
    #region Properties
    protected override bool isServer
    {
      get
      {
        return false;
      }
    }

    public abstract double hashRate
    {
      get;
    }

    public abstract string algorithmName
    {
      get;
    }
    #endregion

    #region Init
    public MiddlewareClient()
    {
      onMessage += MiddlewareClient_onMessage;
    }

    /// <summary>
    /// Sends stats every 3 seconds.
    /// This is a blocking call.
    /// </summary>
    public void Run()
    {
      while (true)
      {
        Send(new MiningStats(algorithmName, hashRate));
        Thread.Sleep(3000);
      }
    }
    #endregion

    #region Events
    void MiddlewareClient_onMessage(
      IMessage message)
    {
      if (message is StartMiningRequest startMiningRequest)
      {
        OnMessage(startMiningRequest);
        
      }
      else if (message is SetSleepFor sleepFor)
      {
        OnMessage(sleepFor);
      }
    }

    public abstract void OnMessage(
      StartMiningRequest startMiningRequest);

    public abstract void OnMessage(
      SetSleepFor sleepFor);
    #endregion
  }
}
