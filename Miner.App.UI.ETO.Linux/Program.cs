using System;
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
      new Application(Platform.Detect).Run(new MainForm());
    }
  }
}
