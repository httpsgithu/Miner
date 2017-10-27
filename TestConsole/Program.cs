using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HD
{
  class Program
  {
    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Unicode)]
    static extern void Run();

    static void Main(string[] args)
    {
      Console.WriteLine("Trying...");

      Run();
      
      Console.WriteLine("Done");
      Console.ReadKey();
    }
  }
}
