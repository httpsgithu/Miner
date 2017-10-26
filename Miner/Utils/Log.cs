using System;

namespace HD
{
  /// <summary>
  /// TODO log to file.
  /// Don't let that log get huge.
  /// </summary>
  public static class Log
  {
    public static void NetworkError(
      string className,
      string method, 
      Exception error)
    {
      Error(nameof(NetworkError), className, method, error);
    }

    static void Error(
      string errorType,
      string className,
      string method,
      Exception error)
    {
      Console.WriteLine($"{errorType}: {className} {method} {error}");
    }

    internal static void ParsingError(
      string className,
      string method,
      Exception error)
    {
      Error(nameof(ParsingError), className, method, error);
    }
  }
}
