using Newtonsoft.Json;
using System;
using System.Threading;

namespace HD
{
  class Program
  {
    static void Main(
      string[] args)
    {
      MiddlewareClient client = new MiddlewareClient();

      // TODO client stat reporting
      int count = 0;
      while (true)
      {
        count++;
        client.Send(new MiningStats("BB", hashRate: count * 100, acceptedHashRate: count));
        Thread.Sleep(3000);
      }
    }
  }
}
