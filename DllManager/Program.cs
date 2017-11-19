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
      client.Run();
    }
  }
}
