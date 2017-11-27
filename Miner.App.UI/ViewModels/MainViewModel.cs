using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;

namespace HD
{
  public class MainViewModel : ViewModel
  {
    #region Data
    public ObservableCollection<MiningStatsBoxViewModel> statsBoxList { get; set; }

    public readonly SettingsVM settings = new SettingsVM();

    readonly Timer timer = new Timer();

    const string windowTitleBase = "HardlyDifficult Miner",
       windowTitleIdle = windowTitleBase + " (idle)",
       windowTitleForStream = windowTitleBase + " (mining for the stream)", // TODO names
       windowTitleForYou = windowTitleBase + " (mining for yourself)";

    public ICommand startStopCMD
    {
      get; set;
    }

    string _startOrStopText;
    #endregion

    #region Properties
    public double cpuUsageForMining
    {
      get
      {
        return HardwareMonitor.percentMinerCPU;
      }
      set { } // TODO read only
    }

    public bool shouldStartWithWindows
    {
      get
      {
        return Miner.instance.settings.minerConfig.shouldStartWithWindows;
      }
      set
      {
        Miner.instance.settings.minerConfig.shouldStartWithWindows = value;
        OnPropertyChanged(nameof(shouldStartWithWindows));
      }
    }

    public double sliderMaxCpuWhileActive
    {
      get
      {
        return Miner.instance.settings.minerConfig.maxCpuWhileActive;
      }
      set
      {
        Miner.instance.settings.minerConfig.maxCpuWhileActive = value;
        OnPropertyChanged(nameof(sliderMaxCpuWhileActive));
        OnPropertyChanged(nameof(sliderMaxCpuWhileIdle));
      }
    }

    public double sliderMaxCpuWhileIdle
    {
      get
      {
        return Miner.instance.settings.minerConfig.maxCpuWhileIdle;
      }
      set
      {
        Miner.instance.settings.minerConfig.maxCpuWhileIdle = value;
        OnPropertyChanged(nameof(sliderMaxCpuWhileActive));
        OnPropertyChanged(nameof(sliderMaxCpuWhileIdle));
      }
    }

    public string startOrStopText
    {
      get
      {
        return _startOrStopText;
      }
      set
      {
        if(_startOrStopText == value)
        {
          return;
        }

        _startOrStopText = value;
        OnPropertyChanged();
      }
    }
    #endregion

    #region Init
    public MainViewModel()
    {
      startStopCMD = new CommandHandler(OnStartStopBtnClick, true);
      // TODO
      //StatsBoxList = new ObservableCollection<MiningStatsBoxViewModel>
      //{
      //  new MiningStatsBoxViewModel(true, StatsBoxUseCase.IntervalEstimatedEarningsFromAllCurrentMiners),
      //  new MiningStatsBoxViewModel(false, StatsBoxUseCase.IntervalEstimatedEarningsFromMe)
      //};

      timer.Interval = 200;
      timer.Elapsed += OnTick;
      timer.Start();
      UpdateRunningState();
    }
    #endregion
    void OnStartStopBtnClick()
    {
      if (Miner.instance.isMinerRunning)
      {
        Stop();
      }
      else
      {
        Start(true);
      }
    }

    void Start(
    bool wasManuallyStarted)
    {
      Miner.instance.Start(wasManuallyStarted);
      UpdateRunningState();
    }

    void Stop()
    {
      Miner.instance.Stop();
      UpdateRunningState();
    }

    void OnTick(object sender, EventArgs e)
    {
      this.FastRefresh();
      Miner.instance.OnTick();
      UpdateRunningState();
    }
    void UpdateRunningState()
    {
      if (Miner.instance.isMinerRunning)
      {
        startOrStopText = "Stop";
      }
      else
      {
        startOrStopText = "Start";
      }
    }

    #region Public
    public void FastRefresh()
    {
      if(HardwareMonitor.RefreshValues() == false)
      { // Miner crashed
        Stop();
      }
      OnPropertyChanged(nameof(cpuUsageForMining));
    }
    #endregion    
  }
}
