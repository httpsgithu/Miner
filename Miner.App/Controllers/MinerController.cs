using System;

namespace HD
{
  public class MinerController
  {
    public readonly MiddlewareServer server;

    public MinerController(
      MiningStatsBoxViewModel viewModel)
    {
      server = new MiddlewareServer(viewModel);
    }
  }
}
