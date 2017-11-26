using System;
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
      new Application(Platform.Detect).Run(new MainForm());
    }
  }
}
