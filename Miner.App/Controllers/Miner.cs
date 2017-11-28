using Miner.OS;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Timers;

namespace HD
{
  /// <summary>
  /// The main class for Mining - settings, start/stop, etc travel through here.
  /// </summary>
  public class Miner
  {
    #region Constants
    static readonly TimeSpan minTimeBetweenStarts = TimeSpan.FromMinutes(5);
    #endregion

    #region Data
    public static readonly Miner instance = new Miner();

    public readonly Settings settings = new Settings();

    public event Action onStatsChange;

    /// <summary>
    /// The current (or most recent) wallet this machine is mining for.
    /// </summary>
    public Beneficiary currentWinner
    {
      get; private set;
    }

    /// <summary>
    /// Do not stop or throttle until the user requests it.
    /// TODO disable throttle
    /// </summary>
    public bool isForceOn
    {
      get; private set;
    }

    Process middlewareProcess;

    DateTime lastConnectionTime;

    DateTime lastStoppedTime;

    readonly MiddlewareServer middlewareServer = new MiddlewareServer();

    readonly MinerAutoStart minerAutoStart = new MinerAutoStart();

    readonly Timer changeWalletTimer = new Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);

    // Network APIs
    readonly APIBitcoinPrice bitcoinPrice = new APIBitcoinPrice();

    readonly APINiceHashMiningPriceList miningPriceList = new APINiceHashMiningPriceList();

    readonly Timer refreshNetworkAPI = new Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
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
        if (isForceOn)
        {
          return 1;
        }
        else if (isCurrentlyIdle)
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

    #region Init
    Miner()
    {
      changeWalletTimer.Elapsed += ChangeWalletTimer_OnTick;

      refreshNetworkAPI.Elapsed += RefreshNetworkAPIsIfCooldown;
      RefreshNetworkAPIsIfCooldown(null, null);
      refreshNetworkAPI.AutoReset = false;
      refreshNetworkAPI.Start();
    }
    #endregion

    #region Events
    /// <summary>
    /// Called anytime any stats change, to fire the event for others.
    /// </summary>
    internal void OnStatsChange()
    {
      onStatsChange?.Invoke();
    }

    /// <summary>
    /// Consider changing wallets every few minutes.
    /// </summary>
    public void ChangeWalletTimer_OnTick(
      object sender,
      ElapsedEventArgs e)
    {
      if (isMinerRunning == false)
      {
        Stop();
        return;
      }

      // Starting again will pick a new winner 
      // If no change, this is a noop
      Start(isForceOn);
    }
    #endregion

    #region Public
    public void Start(
      bool isForceOn)
    {
      if (isForceOn == false)
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
      Debug.Assert(newWinner != null);
      if (currentWinner == newWinner && isMinerRunning)
      { // No change
        return;
      }

      currentWinner = newWinner;
      StartHelper(isForceOn);
    }

    public void Stop()
    {
      changeWalletTimer.Stop();
      if (middlewareProcess != null)
      {
        Log.Event("Stop Miner Process");
        try
        {
          middlewareProcess.Kill();
        }
        catch { }
      }

      lastStoppedTime = DateTime.Now;
      middlewareProcess = null;
      isForceOn = false;
      HardwareMonitor.minerProcessPerformanceCounter = null;
    }
    #endregion

    #region Helpers
    // TODO we need a timer to call this (was from WPF)
    public void RefreshNetworkAPIsIfCooldown(
      object sender, 
      ElapsedEventArgs e)
    {
      bitcoinPrice.ReadWhenReady();
      miningPriceList.ReadWhenReady();
      refreshNetworkAPI.Start();
    }

    void StartHelper(
      bool isForceOn)
    {
      Stop();

      lastConnectionTime = DateTime.Now;

      this.isForceOn = isForceOn;
      // This is where we select the most profitable algorithm...
      middlewareProcess = new Process();
      string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      middlewareProcess.StartInfo.FileName = Path.Combine(
        directory, "Miner.Middleware.Xmr-stak-cpu.exe");

      middlewareProcess.StartInfo.WorkingDirectory = directory;
      middlewareProcess.StartInfo.UseShellExecute = false;
      middlewareProcess.StartInfo.LoadUserProfile = false;
      middlewareProcess.StartInfo.CreateNoWindow = true;

      Log.Event("Start Miner Process");

      middlewareProcess.Start();
      middlewareProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

      MinerOS.instance.RegisterMiddleProcess(middlewareProcess);

      string instanceName = middlewareProcess.GetInstanceName();
      HardwareMonitor.minerProcessPerformanceCounter
        = new PerformanceCounter("Process", "% Processor Time", instanceName);

      changeWalletTimer.Start();
    }
    #endregion
  }
}
