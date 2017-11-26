using JobManagement;
using System;
using System.Diagnostics;
using System.IO;

namespace HD
{
  public class Miner
  {
    #region Data
    public static readonly Miner instance = new Miner();

    public readonly Settings settings = new Settings();

    public static bool isFirstLaunch;

    public event Action onHashRateUpdate;

    public Beneficiary currentWinner
    {
      get; private set;
    }

    //readonly WindowsJob job = new WindowsJob();

    Process middlewareProcess;

    static readonly TimeSpan minTimeBetweenStarts = TimeSpan.FromMinutes(5);

    DateTime lastConnectionTime;

    public bool wasManuallyStarted
    {
      get; private set;
    }

    readonly MinerAutoStart minerAutoStart = new MinerAutoStart();

    DateTime lastStoppedTime;
    #endregion

    #region Properties
    public bool isMinerRunning
    {
      get
      {
        return middlewareProcess?.IsRunning() ?? false;
      }
    }

    public TimeSpan timeSinceLastStopped
    {
      get
      {
        return DateTime.Now - lastStoppedTime;
      }
    }
    #endregion

    #region Events
    internal static void OnFirstLaunch()
    {
      isFirstLaunch = true;
    }

    public void OnTick()
    {
      settings.RefreshNetworkAPIsIfCooldown();
      // TODO shouldn't be driven by ui
      settings.beneficiaries.Refresh();
    }

    internal void OnHashRateUpdate()
    {
      onHashRateUpdate?.Invoke();
    }

    public void OnMinerResultsAccepted()
    {
      if (DateTime.Now - lastConnectionTime < TimeSpan.FromMinutes(5))
      { // Don't switch wallets unless it's been at least 5 minutes.
        return;
      }

      // Starting again will pick a new winner 
      // If no change, this is a noop
      Start(wasManuallyStarted);
    }
    #endregion

    #region Public
    public void Start(
      bool wasManuallyStarted)
    {
      if (wasManuallyStarted == false)
      {
        if (DateTime.Now - lastConnectionTime < minTimeBetweenStarts)
        { // Too soon to auto connect (blocks changing wallets/algorithms)
          return;
        }
      }
      else
      {
        if (isMinerRunning && DateTime.Now - lastConnectionTime < minTimeBetweenStarts)
        { // If running, this is a change wallet request.  Ignore b/c too soon
          return;
        }
      }

      Beneficiary newWinner = settings.beneficiaries.PickAWinner();
      if (currentWinner == newWinner && isMinerRunning)
      { // No change
        return;
      }

      currentWinner = newWinner;
      StartHelper(wasManuallyStarted);
    }

    public void Stop()
    {
      try
      {
        middlewareProcess?.Kill();
      }
      catch { }
      lastStoppedTime = DateTime.Now;
      middlewareProcess = null;
      HardwareMonitor.minerProcessPerformanceCounter = null;
    }
    #endregion

    #region Helpers
    void StartHelper(
      bool wasManuallyStarted)
    {
      lastConnectionTime = DateTime.Now;
      this.wasManuallyStarted = wasManuallyStarted;

      Stop();

      // This is where we select the most profitable algorithm...
      middlewareProcess = new Process();
      middlewareProcess.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "DllManager.exe"); //filename capatalization and extension needed for linux. 
      middlewareProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
      middlewareProcess.StartInfo.UseShellExecute = false;
      middlewareProcess.StartInfo.LoadUserProfile = false;
      middlewareProcess.StartInfo.CreateNoWindow = true;
      middlewareProcess.Start();
      middlewareProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

#if !LINUX
      WindowsJob job = new WindowsJob();
      job.AddProcess(middlewareProcess);
#endif

      string instanceName = middlewareProcess.GetInstanceName();
      HardwareMonitor.minerProcessPerformanceCounter
        = new PerformanceCounter("Process", "% Processor Time",
        instanceName);
    }
    #endregion
  }
}
