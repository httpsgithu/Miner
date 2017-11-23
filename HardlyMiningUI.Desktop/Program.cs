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
            new Application(Platform.Detect).Run(new MainForm());
        }
    }
}
