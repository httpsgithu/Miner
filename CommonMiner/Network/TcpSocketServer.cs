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
      listener.BeginAcceptTcpClient((ar) =>
      {
        onConnect(listener.EndAcceptTcpClient(ar));
      }, null);

      return listener;
    }
  }
}
