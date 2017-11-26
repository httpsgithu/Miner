using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareServer : MinerMiddleware
  {
    readonly MiningStatsBoxViewModel viewModel;
    readonly MinerResourceMonitor monitor;

    protected override bool isServer
    {
      get
      {
        return true;
      }
    }

    public MiddlewareServer(
      MiningStatsBoxViewModel viewModel)
    {
      monitor = new MinerResourceMonitor(this);
      this.viewModel = viewModel;
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
      if(viewModel == null)
      { // TODO
        return;
      }

      MiningStats stats = (MiningStats)message;
      viewModel.btcAmount =
        stats.hashRate
        * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH
        * viewModel.daysPerInterval;
    }
  }
}
