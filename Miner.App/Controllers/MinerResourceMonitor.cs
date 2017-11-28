using System;
using System.Threading;

namespace HD
{
  public class MinerResourceMonitor
  {
    #region Data
    readonly MiddlewareServer server;

    Thread thread;

    long sleepForInNanoseconds = 206892080;

    long deltaSleepForLastFrame;

    int countSameDirection;
    #endregion

    #region Init
    public MinerResourceMonitor(
      MiddlewareServer server)
    {
      Debug.Assert(server != null);

      this.server = server;
    }

    public void Start()
    {
      Debug.Assert(thread == null);

      UpdateSleepFor();
      thread = new Thread(Run);
      thread.Start();
    }

    public void Stop()
    {
      thread?.Abort();
      thread = null;
    }
    #endregion

    #region Private
    void Run()
    {
      while (true)
      {
        if (HardwareMonitor.percentTotalCPU - HardwareMonitor.percentMinerCPU > Miner.instance.currentTargetCpu)
        { // Something else is using the entire budget
          Log.Event($"Miner killed by competing app: target: {Miner.instance.currentTargetCpu} with {HardwareMonitor.percentTotalCPU - HardwareMonitor.percentMinerCPU:p4} consumed by other apps.  Miner was at {HardwareMonitor.percentMinerCPU:p4}");

          Miner.instance.Stop();
          return;
        }
        UpdateSleepFor();

        Thread.Sleep(100);
      }
    }

    void UpdateSleepFor()
    {
      // Possible range is (-1, 1)
      double deltaTargetCpu = HardwareMonitor.percentTotalCPU - Miner.instance.currentTargetCpu;

      if (Math.Abs(deltaTargetCpu) < .025)
      { // Close enough
        countSameDirection = 0;
        deltaSleepForLastFrame = 0;
        return;
      }

      countSameDirection++;
      long delta = (long)(100000 * deltaTargetCpu * countSameDirection);

      if (Math.Sign(deltaTargetCpu) != Math.Sign(deltaSleepForLastFrame) && Math.Sign(deltaSleepForLastFrame) != 0)
      { // Over-corrected, slow down
        deltaSleepForLastFrame = 0;
        countSameDirection = 0;
      }
      else
      {
        deltaSleepForLastFrame = delta;
      }

      if (delta != 0)
      {
        sleepForInNanoseconds += delta;
        if (sleepForInNanoseconds < 0)
        {
          sleepForInNanoseconds = 0;
        }
        server.Send(new SetSleepFor(sleepForInNanoseconds));
      }
    }
    #endregion
  }
}
