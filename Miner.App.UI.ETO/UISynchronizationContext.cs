using System.Threading;
using Eto.Forms;

namespace HD
{
  public class UISynchronizationContext : SynchronizationContext
  {
    private readonly Application application;

    public UISynchronizationContext(Application app)
    {
        application = app;
    }

    public override void Send(SendOrPostCallback d, object state)
    {
        application.Invoke(() => d(state));
    }

    public override void Post(SendOrPostCallback d, object state)
    {
        application.AsyncInvoke(() => d(state));
    }
  }
}
