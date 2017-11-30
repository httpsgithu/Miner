using System;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      Log.Info("Xmr middleware client starting");

      try
      {
        MiddlewareClient client = new XmrMiddlewareClient();
        client.Run();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      Log.Info("Xmr middleware client ending glacefully");
    }
  }
}
