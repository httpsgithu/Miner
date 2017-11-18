using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;

namespace HD
{
  public class ZeroMqEndpoint
  {
    public event Action<string> onMessage;

    public void Start()
    {
      thread = new Thread(poller.Run);
      thread.Start();
    }

    /// <summary>
    /// A blocking alternative to Start.
    /// </summary>
    public void Run()
    {
      poller.Run();
    }

    public void Stop()
    {
      thread?.Abort();
    }

    public void SendMessage(
      string message)
    {
      socket.SendFrame(message);
    }

    // TODO add isConnected, use ping? with trysend

    #region Private
    readonly NetMQSocket socket;

    readonly NetMQPoller poller;

    Thread thread;

    public ZeroMqEndpoint(
      bool isServer,
      int port)
    {
      if (isServer)
      {
        socket = new ResponseSocket($"@tcp://localhost:{port}");
      }
      else
      {
        socket = new RequestSocket($"tcp://localhost:{port}");
      }
      socket.ReceiveReady += Server_ReceiveReady;
      poller = new NetMQPoller { socket };
    }

    void Server_ReceiveReady(
      object sender,
      NetMQSocketEventArgs e)
    {
      string clientMessage = socket.ReceiveFrameString();
      onMessage?.Invoke(clientMessage);
    }
    #endregion
  }
}
