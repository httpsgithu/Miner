using System;
using System.Timers;

namespace HD
{
  /// <summary>
  ///  TODO this is the next feature
  /// </summary>
  public class MinerAutoStart
  {


    public MinerAutoStart()
    {
      Timer timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
      timer.Elapsed += Timer_Elapsed;
      timer.Start();
    }

    void Timer_Elapsed(
      object sender, 
      ElapsedEventArgs e)
    {
      if (
        // Not already running
        Miner.instance.isMinerRunning == false 
        // User has not touched keyboard or mouse in awhile
        && Miner.instance.isCurrentlyIdle
        // The miner did not recently shut down (prevents frequent on/off issues)
        && Miner.instance.timeSinceLastStopped > TimeSpan.FromMinutes(1)
        // The cpu is not already over our max
        && Miner.instance.settings.minerConfig.maxCpuWhileIdle > HardwareMonitor.percentTotalCPU)
      {
        Miner.instance.Start(false);
      }
    }
  }
}