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
      MiddlewareClient client = new XmrMiddlewareClient();
      client.Run();
    }
  }
}
