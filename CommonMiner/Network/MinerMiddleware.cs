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

    public event Action<IMessage> onMessage;

    readonly TcpSocket socket;

    const string ping = "Ping";
    const string pong = "Pong";

    readonly AutoResetEvent pingEvent = new AutoResetEvent(false);

    protected abstract bool isServer { get; }

    #region Init
    public MinerMiddleware()
    {
      socket = new TcpSocket(isServer, OnConnection, OnMessage);
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
      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.TypeNameHandling = TypeNameHandling.All; // TODO is auto okay?
      string json = JsonConvert.SerializeObject(message, settings);
      //IMessage testReturn = JsonConvert.DeserializeObject<IMessage>(json);
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

      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.TypeNameHandling = TypeNameHandling.All; // TODO share this with the server
      IMessage abstractMessage = JsonConvert.DeserializeObject<IMessage>(message, settings);
      onMessage?.Invoke(abstractMessage);
    }
  }
}
