using System;
using System.Net;
using System.Net.Sockets;

namespace HD
{
  public static class TcpSocketClient
  {
    public static TcpClient Connect(
      int port)
    {
      TcpClient client = new TcpClient();
      try
      {
        client.Connect(new IPEndPoint(IPAddress.Loopback, port)); // TODO async
      }
      catch
      {
        // TODO failing here means the server is down, terminate process
        Environment.Exit(123);
      }
      return client;
    }
  }
}
