using System;
using System.Diagnostics;

namespace HD
{
  /// <summary>
  /// Allows WPF and ETO to implement their own dispatcher.
  /// </summary>
  public abstract class MinerUI
  {
    static MinerUI _instance;

    public static MinerUI instance
    {
      get
      {
        if(_instance == null)
        {
          ReflectionHelpers.LoadAllAssemblies();
          _instance = ReflectionHelpers.CreateTheFirst<MinerUI>();
          Debug.Assert(_instance != null);
        }

        return _instance;
      }
    }

    public abstract void Dispatch(
      Action eventToDispatch);
  }
}
