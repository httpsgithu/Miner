using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HD
{
  /// <summary>
  /// Messages
  ///  - Received via onMessage events.
  ///  - Send via Send() or Ping()
  /// </summary>
  public abstract class MinerMiddleware
  {
    public event Action<AbstractMessage> onMessage;

    readonly TcpSocket socket;

    const string ping = "Ping";
    const string pong = "Pong";

    readonly AutoResetEvent pingEvent = new AutoResetEvent(false);

    protected abstract bool isServer { get; }

    #region Init
    public MinerMiddleware()
    {
      socket = new TcpSocket(isServer, OnMessage);
    }
    #endregion

    public bool Ping()
    {
      socket.Send(ping);
      if (pingEvent.WaitOne(50))
      {
        return true;
      }

      return false;
    }

    public void Send(
      AbstractMessage message)
    {
      JsonMessage jsonMessage = new JsonMessage(message);
      string json = JsonConvert.SerializeObject(jsonMessage);
      Send(json);
    }

    /// <summary>
    /// Not for use publicly.. to simplify
    /// </summary>
    void Send(
      string message)
    {
      socket.Send(message);
    }

    void OnMessage(
      string message)
    {
      if (message == ping)
      {
        Send(pong);
        return;
      }
      else if (message == pong)
      { 
        pingEvent.Set();
        return;
      }

      JsonMessage jsonMessage = JsonConvert.DeserializeObject<JsonMessage>(message);
      AbstractMessage abstractMessage = jsonMessage.message;
      onMessage?.Invoke(abstractMessage);
    }
  }
}
