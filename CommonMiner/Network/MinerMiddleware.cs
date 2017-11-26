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
    public event Action onConnection;
    public event Action onDisconnect;

    public event Action<IMessage> onMessage;

    readonly TcpSocket socket;

    const string ping = "Ping";
    const string pong = "Pong";

    readonly AutoResetEvent pingEvent = new AutoResetEvent(false);

    protected abstract bool isServer { get; }

    #region Init
    public MinerMiddleware()
    {
      socket = new TcpSocket(isServer, OnConnection, OnDisconnect, OnMessage);
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

    public void Send<T>(
      T message)
      where T : IMessage
    {
      string json = JsonConvert.SerializeObject(message, Globals.jsonSettings);
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

    void OnConnection()
    {
      onConnection?.Invoke();
    }

    void OnDisconnect()
    {
      onDisconnect?.Invoke();
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
      
      IMessage abstractMessage = JsonConvert.DeserializeObject<IMessage>(message, Globals.jsonSettings);
      onMessage?.Invoke(abstractMessage);
    }
  }
}
