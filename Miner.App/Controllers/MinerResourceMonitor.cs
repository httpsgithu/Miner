using System;
using System.Timers;

namespace HD
{
  public class MinerResourceMonitor
  {
    #region Data
    public event Action onValueUpdated;

    readonly Timer refreshResourcesTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

    /// <summary>
    /// 0 to 1.  1 means sleep always.
    /// </summary>
    double sleepRate;

    DateTime last = DateTime.Now;
    #endregion

    #region Init
    public MinerResourceMonitor()
    {
      refreshResourcesTimer.Elapsed += Refresh;
      refreshResourcesTimer.Start();
    }

    public void Start()
    {
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
          Log.Info("Miner crashed");

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
              Log.Info($"Miner killed by competing app: target: {Miner.instance.currentTargetCpu} with {HardwareMonitor.percentTotalCPU - HardwareMonitor.percentMinerCPU:p4} consumed by other apps.  Miner was at {HardwareMonitor.percentMinerCPU:p4}");

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
        Log.Error(e);
      }
    }

    void UpdateSleepFor()
    {
      if (HardwareMonitor.isMinerDataReady == false)
      {
        sleepRate = .9; // Always start with too much sleep
        Miner.instance.middlewareServer.Send(new SetSleepFor(sleepRate));
        return;
      }

      // Possible range is (-1, 1)
      double deltaTargetCpu = HardwareMonitor.percentTotalCPU - Miner.instance.currentTargetCpu;

      if (Math.Abs(deltaTargetCpu) < .025)
      { // Close enough
        return;
      }

      double targetSleepRate = sleepRate + deltaTargetCpu * .5;
      if (targetSleepRate < 0)
      {
        targetSleepRate = 0;
      }
      else if (targetSleepRate > .9)
      {
        targetSleepRate = .9;
      }

      sleepRate = (sleepRate * 2 + targetSleepRate) / 3;
      Miner.instance.middlewareServer.Send(new SetSleepFor(sleepRate));
    }
    #endregion
  }
}
