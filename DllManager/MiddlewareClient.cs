using Newtonsoft.Json;
using System;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareClient : MinerMiddleware
  {

    protected override bool isServer
    {
      get
      {
        return false;
      }
    }
  }
}
