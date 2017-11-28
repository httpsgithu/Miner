using System;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      Log.Event("Xmr middleware client starting");

      try
      {
        MiddlewareClient client = new XmrMiddlewareClient();
        client.Run();
      }
      catch (Exception e)
      {
        Log.Exception(e);
      }

      Log.Event("Xmr middleware client ending glacefully");
    }
  }
}
