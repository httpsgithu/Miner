using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace HD
{
  public class TcpSocket
  {
    #region Data
    TcpClient client;

    StreamReader reader;

    StreamWriter writer;

    readonly Action<string> onMessage;

    readonly Action onConnection;

    readonly bool isServer;

    TcpListener serverListener;
    #endregion

    public TcpSocket(
      bool isServer,
      Action onConnection,
      Action<string> onMessage)
    {
      this.isServer = isServer;
      this.onConnection = onConnection;
      this.onMessage = onMessage;
      Init();
    }

    void Init()
    {
      if (this.isServer)
      {
        if(serverListener != null)
        {
          serverListener.Stop();
          Thread.Sleep(3000);
        }
        serverListener = TcpSocketServer.Connect(Globals.zeroMqPortServer, (client) =>
        {
          this.client = client;
          Stream stream = client.GetStream();
          reader = new StreamReader(stream);
          writer = new StreamWriter(stream);
          ReadLoop();
          onConnection?.Invoke();
        });
      }
      else
      {
        client = TcpSocketClient.Connect(Globals.zeroMqPortServer);

        try
        {
          Stream stream = client.GetStream();
          reader = new StreamReader(stream);
          writer = new StreamWriter(stream);
          ReadLoop();
        }
        catch
        { // The server is not up ATM
          Environment.Exit(123);
        }
      }
    }

    public void Send(
      string message)
    {
      writer.WriteLine(message);
      writer.Flush();
    }

    async void ReadLoop()
    {
      try
      {
        while (true)
        {
          string message = await reader.ReadLineAsync(); 
          if(message == null)
          { // Disconnected
            break;
          }
          onMessage(message);
        }
      }
      catch (IOException) { }

      try
      {
        client.Close();
      }
      catch { }
    }
  }
}
