using System;
using System.Threading;
using Eto;
using Eto.Forms;

namespace HD
{
  public class Program
  {
    [STAThread]
    public static void Main(string[] args)
    {
      // https://github.com/picoe/Eto/wiki/Running-your-application
      var app = new Application(Platform.Detect);
      app.Run(new MainForm());
    }
  }
}
