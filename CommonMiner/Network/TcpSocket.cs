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

    readonly Action onDisconnect;

    readonly bool isServer;

    TcpListener serverListener;
    #endregion

    public TcpSocket(
      bool isServer,
      Action onConnection,
      Action onDisconnect,
      Action<string> onMessage)
    {
      this.isServer = isServer;
      this.onConnection = onConnection;
      this.onDisconnect = onDisconnect;
      this.onMessage = onMessage;
      Init();
    }

    void Init()
    {
      if (this.isServer)
      {
        if (serverListener != null)
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
          onDisconnect?.Invoke();
          Environment.Exit(123);
        }
      }
    }

    public void Send(
      string message)
    {
      try
      {
        writer.WriteLine(message);
        writer.Flush();
      }
      catch
      {
        try
        {
          client.Close();
        }
        catch { }
        onDisconnect?.Invoke();
      }
    }

    async void ReadLoop()
    {
      try
      {
        while (true)
        {
          string message = await reader.ReadLineAsync();
          if (message == null)
          { // Disconnected
            break;
          }
          onMessage(message);
        }
      }
      catch (IOException e)
      {
        Console.WriteLine(e.ToString());
      }

      try
      {
        client.Close();
      }
      catch { }
      onDisconnect?.Invoke();
    }
  }
}
