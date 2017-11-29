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
    public event Action<MiningStats> onMiningStatsUpdate;

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
      Miner.instance.monitor.Start();
      Send(new StartMiningRequest(
        wallet: Miner.instance.currentWinner.wallet,
        numberOfThreads: Miner.instance.settings.minerConfig.numberOfThreads,
        workerName: Miner.instance.settings.minerConfig.workerName));
    }

    void OnDisconnect(
      Exception e)
    {
      Log.Event($"{nameof(MiddlewareServer)} disconnected with {e}");

      Miner.instance.monitor.Stop();
    }

    void OnMessage(
      object message)
    {
      Debug.Assert(message != null);

      if (message is MiningStats stats)
      {
        onMiningStatsUpdate?.Invoke(stats);
        // TODO UI
        //viewModel.btcAmount =
        //  stats.hashRate
        //  * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH
        //  * viewModel.daysPerInterval;
      }
      else
      {
        Debug.Fail("Missing message type");
      }
    }
    #endregion
  }
}
