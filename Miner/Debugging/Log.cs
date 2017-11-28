using System;
using System.IO;

namespace HD
{
  /// <summary>
  /// TODO Don't let that log get huge.
  /// twitchloff: Just suggestions:put log files in a logs folder. Write some logmessage to show when server starts, prefferably multiple lines. Then use tail -F or a log program like logfusion to tail the files. The logfiles can just stay open. No ned to delete/reopen files
  /// 
  /// Consider serilog
  /// </summary>
  public static class Log
  {
    const string 
      filename = "log.log",
      eventFile = "event.log";

    public static void Assert(
      string message)
    {
      LogMessage(message);
    }

    public static void NetworkError(
      string className,
      string method, 
      Exception error)
    {
      Error(nameof(NetworkError), className, method, error);
    }

    public static void ParsingError(
      string className,
      string method,
      Exception error)
    {
      Error(nameof(ParsingError), className, method, error);
    }

    static void Error(
      string errorType,
      string className,
      string method,
      Exception error)
    {
      LogMessage($"{errorType}: {className} {method} {error}");
    }

    static void LogMessage(
      string message)
    {
      ToFile(filename, message);
    }

    internal static void ToFile(
      string logFile, 
      string message)
    {
      lock(logFile)
      {
        File.AppendAllText(logFile, $"{DateTime.Now}: {message}{Environment.NewLine}");
      }
    }

    public static void Event(
      string message)
    {
      ToFile(eventFile, message);
    }
  }
}
