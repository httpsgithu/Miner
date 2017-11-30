using System;
using System.Timers;

namespace HD
{
  public class MinerResourceMonitor
  {
    #region Data
    public event Action onValueUpdated;

    readonly MiddlewareServer server;

    readonly Timer refreshResourcesTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

    long sleepForInNanoseconds;

    long deltaSleepForLastFrame;

    int countSameDirection;

    DateTime last = DateTime.Now;
    #endregion

    #region Init
    public MinerResourceMonitor(
      MiddlewareServer server)
    {
      Debug.Assert(server != null);

      this.server = server;
      refreshResourcesTimer.Elapsed += Refresh;
      refreshResourcesTimer.Start();
    }

    public void Start()
    {
      sleepForInNanoseconds = 206892080;
      UpdateSleepFor();
    }
    #endregion

    #region Private
    void Refresh(
      object sender,
      ElapsedEventArgs eargs)
    {
      try
      {
        if (HardwareMonitor.RefreshValues() == false
          && Miner.instance.isMinerRunning)
        { // Miner crashed
          Log.Event("Miner crashed");

          Miner.instance.Stop();
          return;
        }
        onValueUpdated?.Invoke();

        if (HardwareMonitor.isMinerDataReady)
        {
          if (HardwareMonitor.percentTotalCPU - HardwareMonitor.percentMinerCPU > Miner.instance.currentTargetCpu)
          { // Something else is using the entire budget consistently for at least 2 seconds (sleep by 1)
            if (DateTime.Now - last > TimeSpan.FromSeconds(1.5))
            {
              Log.Event($"Miner killed by competing app: target: {Miner.instance.currentTargetCpu} with {HardwareMonitor.percentTotalCPU - HardwareMonitor.percentMinerCPU:p4} consumed by other apps.  Miner was at {HardwareMonitor.percentMinerCPU:p4}");

              Miner.instance.Stop();
              return;
            }
          }
          else
          {
            last = DateTime.Now;
          }
          UpdateSleepFor();
        }
        else
        {
          last = DateTime.Now;
        }
      }
      catch (Exception e)
      {
        Log.Exception(e);
      }
    }

    void UpdateSleepFor()
    {
      if (HardwareMonitor.isMinerDataReady == false)
      {
        server.Send(new SetSleepFor(sleepForInNanoseconds));
        return;
      }

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
