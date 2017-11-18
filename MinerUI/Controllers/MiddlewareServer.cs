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
      onMessage += MiddlewareServer_onMessage;
    }

    void MiddlewareServer_onMessage(
      AbstractMessage message)
    {
      MiningStats stats = (MiningStats)message;
      viewModel.btcAmount = stats.hashRate * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH
        * viewModel.daysPerInterval;
    }
  }
}
