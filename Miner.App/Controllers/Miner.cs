using Miner.instance.OS;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
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

    public readonly Settings settings;

    public event Action onStatsChange;

    public event Action onStartOrStop;

    public readonly MinerResourceMonitor monitor;

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

    DateTime lastStartedTime;

    DateTime lastStoppedTime;

    public readonly MiddlewareServer middlewareServer;

    readonly MinerAutoStart minerAutoStart;

    readonly System.Timers.Timer changeWalletTimer = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);

    // Network APIs
    readonly APIBitcoinPrice bitcoinPrice;

    readonly APINiceHashMiningPriceList miningPriceList;

    readonly System.Timers.Timer refreshNetworkAPI = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
    #endregion

    #region Properties
    public decimal dollarPerBitcoin
    {
      get
      {
        return new decimal(bitcoinPrice.dollarPerBitcoin);
      }
    }

    public decimal pricePerDayInBtcFor1MHOfCryptonight
    {
      get
      {
        return new decimal(miningPriceList.pricePerDayInBtcFor1MHOfCryptonight);
      }
    }

    public bool isMinerRunning
    {
      get
      {
        return middlewareProcess?.IsRunning() ?? false;
      }
    }

    /// <summary>
    /// True if the user has been away for at least timeTillIdle.
    /// </summary>
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
          return settings.minerConfig.maxCpuWhileIdle;
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

    public TimeSpan timeSinceLastStarted
    {
      get
      {
        return DateTime.Now - lastStartedTime;
      }
    }
    #endregion

    #region Init
    Miner()
    {
      settings = new Settings();
      middlewareServer = new MiddlewareServer();
      minerAutoStart = new MinerAutoStart();
      bitcoinPrice = new APIBitcoinPrice();
      miningPriceList = new APINiceHashMiningPriceList();

      changeWalletTimer.Elapsed += ChangeWalletTimer_OnTick;

      refreshNetworkAPI.Elapsed += RefreshNetworkAPIsIfCooldown;
      refreshNetworkAPI.AutoReset = false;
      RefreshNetworkAPIsIfCooldown(null, null);
      monitor = new MinerResourceMonitor();
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
        if (DateTime.Now - lastStartedTime < minTimeBetweenStarts)
        { // Too soon to auto connect (blocks changing wallets/algorithms)
          return;
        }
      }
      else
      {
        if (isMinerRunning && DateTime.Now - lastStartedTime < minTimeBetweenStarts)
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

      Debug.Assert(newWinner.isValidAndActive);

      currentWinner = newWinner;
      StartHelper(isForceOn);
    }

    public void Stop()
    {
      HardwareMonitor.minerProcessPerformanceCounter = null;
      changeWalletTimer.Stop();
      if (middlewareProcess != null)
      {
        Log.Info("Stop Miner Process");
        try
        {
          middlewareProcess.Kill();
        }
        catch { }
        middlewareProcess = null;
      }

      lastStoppedTime = DateTime.Now;
      isForceOn = false;
      onStartOrStop?.Invoke();
    }
    #endregion

    #region Helpers
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

      lastStartedTime = DateTime.Now;

      this.isForceOn = isForceOn;
      // This is where we select the most profitable algorithm...
      try
      {

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

        string instanceName = middlewareProcess.GetInstanceName();

        MinerOS.instance.RegisterMiddleProcess(middlewareProcess);

        Log.Info("Start Miner Process");

        changeWalletTimer.Start();
        onStartOrStop?.Invoke();
        HardwareMonitor.minerProcessPerformanceCounter
          = new PerformanceCounter("Process", "% Processor Time", instanceName);
      }
      catch(Exception e)
      {
        Log.Error(e);

        HardwareMonitor.minerProcessPerformanceCounter = null;
        try
        {
          middlewareProcess?.Kill();
        }
        catch { }
        middlewareProcess = null;
      }
    }
    #endregion
  }
}
