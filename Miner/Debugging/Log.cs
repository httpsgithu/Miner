using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HD
{
  public static class Log
  {
    static Log()
    {
      LoggerConfiguration config = new LoggerConfiguration();
      config.WriteTo.RollingFile(
          pathFormat: Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Assembly.GetEntryAssembly().FullName + "{Date}.log"),
          outputTemplate: "{Timestamp} [{Level}] {Message}{NewLine}{Exception}",
          retainedFileCountLimit: 5,
          buffered: false,
          shared: false);

      Serilog.Log.Logger = config.CreateLogger();
    }
    
    public static void Info(
      string message = null,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      Serilog.Log.Information($"{message} from {memberName} @ {sourceFilePath}: {sourceLineNumber}");
    }

    public static void Warning(
      string message,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      Serilog.Log.Warning($"{message} from {memberName} @ {sourceFilePath}: {sourceLineNumber}");
    }

    public static void Error(
      Exception e,
      string message = null,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      Serilog.Log.Error(e, $"{message} from {memberName} @ {sourceFilePath}: {sourceLineNumber}");
    }

    public static void Error(
      string message = null,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      Serilog.Log.Error($"{message} from {memberName} @ {sourceFilePath}: {sourceLineNumber}");
    }
  }
}
