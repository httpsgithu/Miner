using System;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareServer : MinerMiddleware
  {
    public event Action<MiningStats> onMiningStatsUpdate;
    readonly MinerResourceMonitor monitor;

    protected override bool isServer
    {
      get
      {
        return true;
      }
    }

    public MiddlewareServer()
    {
      monitor = new MinerResourceMonitor(this);
      onConnection += OnConnection;
      onDisconnect += OnDisconnect;
      onMessage += OnMessage;
    }

    void OnConnection()
    {
      monitor.Start();
      Send(new StartMiningRequest(
        wallet: Miner.instance.currentWinner.wallet,
        numberOfThreads: Miner.instance.settings.minerConfig.numberOfThreads,
        workerName: Miner.instance.settings.minerConfig.workerName));
    }

    void OnDisconnect()
    {
      monitor.Stop();
    }

    void OnMessage(
      IMessage message)
    {
      onMiningStatsUpdate?.Invoke((MiningStats)message);
      // TODO
      //viewModel.btcAmount =
      //  stats.hashRate
      //  * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH
      //  * viewModel.daysPerInterval;
    }
  }
}
