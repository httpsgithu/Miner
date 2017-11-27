using Miner.OS;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HD
{
  public class Miner
  {
    #region Data
    public static readonly Miner instance = new Miner();

    readonly MiddlewareServer middlewareServer = new MiddlewareServer();

    public readonly Settings settings = new Settings();

    public static bool isFirstLaunch;

    public event Action onHashRateUpdate;

    public Beneficiary currentWinner
    {
      get; private set;
    }

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

    public bool isCurrentlyIdle
    {
      get
      {
        return MinerOS.instance.idleTime >= settings.minerConfig.timeTillIdle;
      }
    }

    public double currentTargetCpu
    {
      get
      {
        if (isCurrentlyIdle)
        {
          return settings.minerConfig.maxCpuWhileIdle;
        }
        else
        {
          return settings.minerConfig.maxCpuWhileActive;
        }
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
      // TODO shouldn't be driven by ui
      //settings.RefreshNetworkAPIsIfCooldown();
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
      string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      middlewareProcess.StartInfo.FileName = Path.Combine(
        directory, "Miner.Middleware.Xmr-stak-cpu.exe");

      middlewareProcess.StartInfo.WorkingDirectory = directory;
      middlewareProcess.StartInfo.UseShellExecute = false;
      middlewareProcess.StartInfo.LoadUserProfile = false;
      middlewareProcess.StartInfo.CreateNoWindow = true;
      middlewareProcess.Start();
      middlewareProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

      MinerOS.instance.RegisterMiddleProcess(middlewareProcess);

      string instanceName = middlewareProcess.GetInstanceName();
      HardwareMonitor.minerProcessPerformanceCounter
        = new PerformanceCounter("Process", "% Processor Time",
        instanceName);
    }
    #endregion
  }
}
