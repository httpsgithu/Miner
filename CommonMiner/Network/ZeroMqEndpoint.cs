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
      thread.Abort();
    }

    public void SendMessage(
      string message)
    {
      server.SendFrame(message);
    }

    // TODO add isConnected, use ping? with trysend

    #region Private
    readonly ResponseSocket server;

    readonly NetMQPoller poller;

    Thread thread;

    public ZeroMqEndpoint(
      bool isServer)
    {
      char prefix = isServer ? '@' : '>';
      server = new ResponseSocket($"{prefix}tcp://localhost:{Globals.zeroMqPort}");
      server.ReceiveReady += Server_ReceiveReady;
      poller = new NetMQPoller { server };
    }

    void Server_ReceiveReady(
      object sender,
      NetMQSocketEventArgs e)
    {
      string clientMessage = server.ReceiveFrameString();
      onMessage?.Invoke(clientMessage);
    }
    #endregion
  }
}
