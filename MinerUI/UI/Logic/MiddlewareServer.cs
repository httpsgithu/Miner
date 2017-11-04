using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareServer
  {
    public event Action<JsonMessage> onMessage;

    readonly ZeroMqEndpoint endpoint;

    readonly MiningStatsBoxViewModel viewModel;

    public MiddlewareServer(
      MiningStatsBoxViewModel viewModel)
    {
      this.viewModel = viewModel;
      endpoint = new ZeroMqEndpoint(true);
      endpoint.onMessage += Endpoint_onMessage;
      endpoint.Start();
    }

    void Endpoint_onMessage(
      string message)
    {
      MiningStats stats = JsonConvert.DeserializeObject<MiningStats>(message);
      viewModel.btcAmount = stats.hashRate * Miner.instance.settings.miningPriceList.pricePerDayInBtcFor1MH * viewModel.daysPerInterval;
    }

    public void Stop()
    {
      endpoint.Stop();
    }
  }
}
