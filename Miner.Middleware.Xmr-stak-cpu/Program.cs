using System;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      Log.Event("Xmr middleware client starting");

      MiddlewareClient client = new XmrMiddlewareClient();
      client.Run();

      Log.Event("Xmr middleware client ending glacefully");
    }
  }
}
