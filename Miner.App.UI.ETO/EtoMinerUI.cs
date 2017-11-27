using System;

namespace HD
{
  public class EtoMinerUI : MinerUI
  {
    public override void Dispatch(
      Action eventToDispatch)
    {
      // TODO ETO specific dispatcher
      // Dispatcher.CurrentDispatcher.Invoke(eventToDispatch);
    }
  }
}
