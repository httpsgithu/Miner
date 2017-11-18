using Newtonsoft.Json;
using System;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      // TODO on command
      //XmrDll dll = new XmrDll();
      //dll.StartMining("14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE", 1);
      MiddlewareClient client = new MiddlewareClient();

      int count = 0;

      while (true)
      {
        Console.ReadKey();

        count++;
        client.Send(new MiningStats("BB", count * 100, count));

        Console.WriteLine("Ok");
      }

      //Console.WriteLine("Fail...");
      //Console.ReadLine();
    }
  }
}
