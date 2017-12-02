using System;
using HD;
using System.Diagnostics;

namespace Miner.instance.OS
{
  /// <summary>
  /// Offers an abstract interface to any platform specific features.
  /// The dll for the current platform is loaded from the same directory via reflection.
  /// There must be only one MinerOS in the build's directory for this to work.
  /// </summary>
  public abstract class MinerOS
  {
    static MinerOS _instance;

    public static MinerOS instance
    {
      get
      {
        if(_instance == null)
        {
          ReflectionHelpers.LoadAllAssemblies();
          _instance = ReflectionHelpers.CreateTheFirst<MinerOS>();
          Debug.Assert(_instance != null);
        }

        return _instance;
      }
    }

    public abstract bool shouldAutoStart { get; set; }
    public abstract TimeSpan idleTime { get; }

    public abstract void RegisterMiddleProcess(
      Process middlewareProcess);
  }
}
