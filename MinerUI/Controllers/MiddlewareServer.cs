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
      this.viewModel = viewModel;
      onConnection += OnConnection;
      onMessage += OnMessage;
    }

    void OnConnection()
    {
      Send(new StartMiningRequest(
        wallet: Miner.instance.currentWinner.wallet,
        numberOfThreads: Miner.instance.settings.minerConfig.numberOfThreadsWhenIdle, // TODO select active or idle
        workerName: Miner.instance.settings.minerConfig.workerName));
    }

    void OnMessage(
      IMessage message)
    {
      MiningStats stats = (MiningStats)message;
      viewModel.btcAmount = stats.hashRate * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH
        * viewModel.daysPerInterval;
    }
  }
}
