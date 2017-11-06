using System;

namespace HD
{
  public class MinerController
  {
    readonly MiddlewareServer server;

    public MinerController()
    {
      //server = new MiddlewareServer(((MainViewModel)mainWindow.DataContext).StatsBoxList[1]);
    }

    internal void Stop()
    {
      server.Stop();
    }
  }
}
