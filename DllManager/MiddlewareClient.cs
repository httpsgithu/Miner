using Newtonsoft.Json;
using System;

namespace HD
{
  /// <summary>
  /// Adds the JSON layer
  /// </summary>
  public class MiddlewareClient
  {
    public event Action<JsonMessage> onMessage;

    readonly ZeroMqEndpoint endpoint;

    public MiddlewareClient()
    {
      endpoint = new ZeroMqEndpoint(false);
      endpoint.onMessage += Endpoint_onMessage;
    }

    public void Run()
    {
      endpoint.Run();
    }

    void Endpoint_onMessage(
      string message)
    {
      // todo
    }

    public void Stop()
    {
      endpoint.Stop();
    }
  }
}
