using System;
using System.Diagnostics;
using System.Threading;

namespace HD
{
  public static class HardwareMonitor
  {
    #region Data
    const int numberOfSamples = 3;

    readonly static RollingHistory totalCpuRollingHistory = new RollingHistory(numberOfSamples);

    readonly static RollingHistory miningCpuRollingHistory = new RollingHistory(numberOfSamples);

    /// <summary>
    /// This appears to return results 10% different than the Windows Task Manager.... ?
    /// </summary>
    static readonly PerformanceCounter totalProcessorTime
      = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    #endregion

    static PerformanceCounter _minerProcessPerformanceCounter;

    public static PerformanceCounter minerProcessPerformanceCounter
    {
      private get
      {
        return _minerProcessPerformanceCounter;
      }
      set
      {
        _minerProcessPerformanceCounter = value;

        if(value == null)
        {
          miningCpuRollingHistory.Clear();
        }
      }
    }

    public static double percentTotalCPU
    {
      get
      {
        if(totalCpuRollingHistory.hasFilledOnce == false)
        {
          return 0;
        }

        return totalCpuRollingHistory.value / 100;
      }
    }

    public static double percentMinerCPU
    {
      get
      {
        if(miningCpuRollingHistory.hasFilledOnce == false)
        {
          return 0;
        }

        return miningCpuRollingHistory.value / 100;
      }
    }

    /// <summary>
    /// Returns false if the process crashed.
    /// </summary>
    public static bool RefreshValues()
    {
      totalCpuRollingHistory.Add(totalProcessorTime.NextValue());
      if (minerProcessPerformanceCounter != null)
      {
        try
        {
          miningCpuRollingHistory.Add(minerProcessPerformanceCounter.NextValue() / Environment.ProcessorCount);
        }
        catch
        {
          // The middleware app has crashed
          minerProcessPerformanceCounter = null;
          Log.Event("Clearing the history #1");
          miningCpuRollingHistory.Clear();
          return false;
        }
      }
      else
      {
        Log.Event("Clearing the history #2");
        miningCpuRollingHistory.Clear();
      }

      return true;
    }
  }
}
