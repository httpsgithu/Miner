using System;

namespace HD
{
  public class MinerController
  {
    readonly MiddlewareServer server;

    public MinerController(
      MiningStatsBoxViewModel viewModel)
    {
      server = new MiddlewareServer(viewModel);
    }
  }
}
