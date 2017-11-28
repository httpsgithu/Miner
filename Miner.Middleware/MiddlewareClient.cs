using System.Threading;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public abstract class MiddlewareClient : MinerMiddleware
  {
    #region Constants
    protected override bool isServer
    {
      get
      {
        return false;
      }
    }
    #endregion

    #region Properties
    // TODO units are not clear and may not be consistent across algorithms.
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
      onMessage += OnMessage;
    }

    /// <summary>
    /// Sends stats every so often.
    /// This is a blocking call.
    /// </summary>
    public void Run()
    {
      while (true)
      {
        Send(new MiningStats(algorithmName, hashRate));
        Thread.Sleep(1000);
      }
    }
    #endregion

    #region Events
    void OnMessage(
      object message)
    {
      if (message is StartMiningRequest startMiningRequest)
      {
        OnMessage(startMiningRequest);
      }
      else if (message is SetSleepFor sleepFor)
      {
        OnMessage(sleepFor);
      }
      else
      {
        Debug.Fail($"Missing message handler {message}");
      }
    }

    public abstract void OnMessage(
      StartMiningRequest startMiningRequest);

    public abstract void OnMessage(
      SetSleepFor sleepFor);
    #endregion
  }
}
