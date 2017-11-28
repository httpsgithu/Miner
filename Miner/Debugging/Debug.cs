using System;
using System.Runtime.CompilerServices;

namespace HD
{
  public static class Debug
  {
    // TODO conditional
    // TODO string format
    public static void Assert(
      bool isTrue,
      string message = null,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      if (isTrue == false)
      {
        Log.Assert($"{nameof(Assert)} failed {message} {memberName} @ {sourceFilePath}: {sourceLineNumber}");
      }
    }

    public static void Fail(
      string message = null,
      [CallerMemberName] string memberName = null,
      [CallerFilePath] string sourceFilePath = null,
      [CallerLineNumber] int sourceLineNumber = 0)
    {
      Log.Assert($"{nameof(Fail)} {message} {memberName} @ {sourceFilePath}: {sourceLineNumber}");
    }
  }
}
