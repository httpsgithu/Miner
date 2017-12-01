using System;

namespace HD
{
  public class MiddlewareServer : MinerMiddleware
  {
    #region Constants
    protected override bool isServer
    {
      get
      {
        return true;
      }
    }
    #endregion

    #region Data
    public static event Action<MiningStats> onMiningStatsUpdate;

    public event Action<WorkIsStalled> onWorkIsStalled;
    #endregion

    #region Init
    public MiddlewareServer()
    {
      onConnection += OnConnection;
      onDisconnect += OnDisconnect;
      onMessage += OnMessage;
    }
    #endregion

    #region Events
    void OnConnection()
    {
      Miner.instance.resourceMonitor.Start();
      Send(new StartMiningRequest(
        wallet: Miner.instance.currentWinner.wallet,
        numberOfThreads: Miner.instance.settings.minerConfig.numberOfThreads,
        workerName: Miner.instance.settings.minerConfig.workerName,
        stratumUrl: Miner.instance.regionMonitor.currentRegion.stratumUrl));
    }

    void OnDisconnect(
      Exception e)
    {
      Log.Info($"{nameof(MiddlewareServer)} disconnected with {e}");
    }

    void OnMessage(
      object message)
    {
      Debug.Assert(message != null);

      if (message is MiningStats stats)
      {
        onMiningStatsUpdate?.Invoke(stats);
      }
      else if (message is WorkIsStalled stalled)
      {
        onWorkIsStalled?.Invoke(stalled);
      }
      else
      {
        Debug.Fail("Missing message type");
      }
    }
    #endregion
  }
}
