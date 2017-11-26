using System;
using System.Net;
using System.Net.Sockets;

namespace HD
{
  public static class TcpSocketServer
  {
    public static TcpListener Connect(
      int port,
      Action<TcpClient> onConnect)
    {
      TcpListener listener = new TcpListener(IPAddress.Loopback, port);
      listener.Start();
      BeginConnection(onConnect, listener);

      return listener;
    }

    static void BeginConnection(
      Action<TcpClient> onConnect, 
      TcpListener listener)
    {
      listener.BeginAcceptTcpClient((ar) =>
      {
        onConnect(listener.EndAcceptTcpClient(ar));
        BeginConnection(onConnect, listener);
      }, null);
    }
  }
}
