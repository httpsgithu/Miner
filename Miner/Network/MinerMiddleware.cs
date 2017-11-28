using System;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace HD
{
  /// <summary>
  /// Messages
  ///  - Received via onMessage events.
  ///  - Send via Send() or Ping()
  /// </summary>
  public abstract class MinerMiddleware
  {
    #region Constants
    const string
      serverLogFile = "Server.log",
      clientLogFile = "Client.log";
    #endregion

    #region Data
    public event Action<object> onMessage;

    readonly TcpSocket socket;
    #endregion

    #region Properties
    public event Action onConnection
    {
      add
      {
        socket.onConnection += value;
      }
      remove
      {
        socket.onConnection -= value;
      }
    }

    public event Action<Exception> onDisconnect
    {
      add
      {
        socket.onDisconnect += value;
      }
      remove
      {
        socket.onDisconnect -= value;
      }
    }

    protected abstract bool isServer { get; }

    string logFile
    {
      get
      {
        return isServer ? serverLogFile : clientLogFile;
      }
    }
    #endregion

    #region Init
    public MinerMiddleware()
    {
      if (isServer)
      {
        socket = new TcpSocketServer(MinerGlobalVariables.internalServerPort);
      }
      else
      {
        socket = new TcpSocketClient(MinerGlobalVariables.internalServerPort);
      }
      socket.onMessage += OnMessage;
    }
    #endregion

    #region Events
    void OnMessage(
      string message)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(message) == false);

      object abstractMessage = JsonConvert.DeserializeObject<object>(message, MinerGlobalVariables.jsonSettings);
      Debug.Assert(abstractMessage != null);
      Debug.Assert(abstractMessage.GetType() != typeof(object));

      onMessage?.Invoke(abstractMessage);
    }
    #endregion

    #region Write
    public void Send(
      object message)
    {
      Debug.Assert(message != null);

      string json = JsonConvert.SerializeObject(message, MinerGlobalVariables.jsonSettings);
      // Perf: this is a lot of logging
      Log.ToFile(logFile, json);
      Debug.Assert(string.IsNullOrWhiteSpace(json) == false);

      SendString(json);
    }
    #endregion

    #region Private Write
    void SendString(
      string message)
    {
      Debug.Assert(string.IsNullOrWhiteSpace(message) == false);

      socket.Send(message);
    }
    #endregion
  }
}
