using System;
using System.Diagnostics;
using HD;
using JobManagement;

namespace Miner.instance.OS.Windows
{
  class WindowsMinerOS : MinerOS
  {
    public override bool shouldAutoStart
    {
      get
      {
        return StartWithWindows.shouldStartWithWindows;
      }
      set
      {
        StartWithWindows.shouldStartWithWindows = value;
      }
    }

    public override TimeSpan idleTime
    {
      get
      {
        return IdleTimeFinder.GetIdleTime();
      }
    }

    public override void RegisterMiddleProcess(
      Process middlewareProcess)
    {
      WindowsJob job = new WindowsJob();
      job.AddProcess(middlewareProcess);
    }
  }
}
