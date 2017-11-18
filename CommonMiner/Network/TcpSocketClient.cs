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
        client.Connect(new IPEndPoint(IPAddress.Loopback, port)); 
      }
      catch
      {
        // Failing here means the server is down, terminate process
        Environment.Exit(321);
      }
      return client;
    }
  }
}
