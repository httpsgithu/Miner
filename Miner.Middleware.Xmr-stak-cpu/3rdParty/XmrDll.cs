using System;
using System.Runtime.InteropServices;

namespace HD
{
  public class XmrDll
  {
    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
               BestFitMapping = false,
               ThrowOnUnmappableChar = true)]
    static extern bool Start(
      string configurationJson);

    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
               BestFitMapping = false,
               ThrowOnUnmappableChar = true)]
    static extern double GetTotalHashRate();

    [DllImport("xmr-stak-cpu.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
               BestFitMapping = false,
               ThrowOnUnmappableChar = true)]
    static extern void SleepFor(long sleepTimeInNanoseconds);

    public double totalHashRate
    {
      get
      {
        return GetTotalHashRate() / 1000000;
      }
    }

    public void StartMining(
      string configurationJson)
    {
      // Requires whitespace at the start of the string.
      Start(configurationJson);
    }

    public void SetSleepFor(
      long sleepTimeInNanoseconds)
    {
      SleepFor(sleepTimeInNanoseconds);
    }
  }
}


















//        int indexOfEndSpace = message.IndexOf(" ", indexOfEnd - 2);
//        string valueString = message.Substring(indexOfSpace, indexOfEndSpace - indexOfSpace);
//        if (double.TryParse(valueString, out currentHashRateMHpS))
//        {
//          string unit = message.Substring(indexOfEndSpace + 1, indexOfEnd - indexOfEndSpace - 1);
//          if (unit.Equals("H", StringComparison.InvariantCulture))
//          {
//            currentHashRateMHpS /= 1000000;
//          }
//          else if (unit.Equals("KH", StringComparison.InvariantCulture))
//          {
//            currentHashRateMHpS /= 1000;
//          }
//          else
//          {
//            Console.WriteLine("Hash rate unit not recognized");
//          }

//          OnHashRateUpdate();
//        }
//      }