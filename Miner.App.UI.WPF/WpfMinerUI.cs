using System;
using System.Windows.Threading;

namespace HD
{
  public class WpfMinerUI : MinerUI
  {
    public override void Dispatch(
      Action eventToDispatch)
    {
      Dispatcher.CurrentDispatcher.Invoke(eventToDispatch);
    }
  }
}
