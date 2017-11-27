using System;
using System.Threading;
using Eto;
using Eto.Forms;

namespace HardlyMiningUI.Desktop
{
  public class Program
  {
    [STAThread]
    public static void Main(string[] args)
    {
      // https://github.com/picoe/Eto/wiki/Running-your-application
      var app = new Application(Platform.Detect);
      SynchronizationContext.SetSynchronizationContext(new UISynchronizationContext(app));
      app.Run(new MainForm());
    }
  }
}
