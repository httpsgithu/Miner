using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner.OS.Linux
{
  public class LinuxMinerOS : MinerOS
  {
    /// <summary>
    /// TODO start when the OS starts.
    /// 
    /// Note that this is stored by the OS, and not in our app settings.
    /// </summary>
    public override bool shouldAutoStart
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    /// <summary>
    /// TODO return how long it has been since the user touched either the mouse or the keyboard.
    /// </summary>
    public override TimeSpan idleTime
    {
      get
      {
        return TimeSpan.FromMinutes(9999999);
      }
    }

    /// <summary>
    /// TODO this should connect the given process to the current process
    /// so that if the current process dies, the given process is killed
    ///
    /// I believe this is just a nice to have.
    /// </summary>
    /// <param name="middlewareProcess"></param>
    public override void RegisterMiddleProcess(
      Process middlewareProcess) { }
  }
}
