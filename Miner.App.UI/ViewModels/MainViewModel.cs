using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Input;
using System.Reflection;

namespace HD
{
  public class MainViewModel : ViewModel
  {
    #region Data
    public ObservableCollection<MiningStatsBoxViewModel> statsBoxList
    {
      get;
    }

    public SettingsViewModel settings
    {
      get;
    }

    public ICommand startStopCMD
    {
      get; set;
    }

    string _startOrStopText;

    bool _shouldShowBtcVsD;
    #endregion

    #region Properties
    public IEnumerable<MiningStatsBoxViewModel> additionalWalletStats
    {
      get
      {
        for (int i = 1; i < statsBoxList.Count; i++)
        {
          yield return statsBoxList[i];
        }
      }
    }

    public MiningStatsBoxViewModel currentlyMiningFor
    {
      get
      {
        if (isCurrentlyMining)
        {
          return statsBoxList[0];
        }

        return null;
      }
    }

    public int cpuUsageForMining0To100000
    {
      get
      {
        return (int)(cpuUsageForMining * 100000);
      }
    }

    public double cpuUsageForMining
    {
      get
      {
        return HardwareMonitor.percentMinerCPU;
      }
      set { } // TODO read only
    }
    public string cpuUsageMiningText
    {
        get
        {
            return "CPU Mining % Usage: " + (HardwareMonitor.percentMinerCPU * 100).ToString("F");
            }
    }

    public int cpuUsageOverall0to100000
    {
        get
        {
            return (int)(HardwareMonitor.percentTotalCPU * 100000);
        }
        set { }
    }

    public double cpuUsageOverall
    {
      get
      {
        return HardwareMonitor.percentTotalCPU;
      }
      set { }
    }

    public string cpuUsageText
    {
        get
        {
            return "CPU Total % Usage: " + (HardwareMonitor.percentTotalCPU * 100).ToString("F");
        }
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
        if (_startOrStopText == value)
        {
          return;
        }

        _startOrStopText = value;
        OnPropertyChanged();
      }
    }

    public bool shouldShowBtcVsD
    {
      get
      {
        return _shouldShowBtcVsD;
      }
      set
      {
        _shouldShowBtcVsD = value;
        OnPropertyChanged();
      }
    }

    public bool isCurrentlyMining
    {
      get
      {
        return Miner.instance.isMinerRunning;
      }
    }
    #endregion

    #region Init
    public MainViewModel()
    {
      settings = new SettingsViewModel();

      startStopCMD = new CommandHandler(OnStartStopBtnClick, true);
      statsBoxList = new ObservableCollection<MiningStatsBoxViewModel>
      {
        new CurrentMiningStatsViewModel(this),
      };

      // TODO support add/remove from this list
      foreach (Beneficiary beneficiary in Miner.instance.settings.beneficiaries)
      {
        if (beneficiary.isValidAndActive)
        {
          statsBoxList.Add(new NetworkMiningStatsViewModel(this, beneficiary));
        }
      }

      Miner.instance.resourceMonitor.onValueUpdated += OnMonitorValueUpdated;

      Miner.instance.onStartOrStop += OnMinerStartOrStop;
      Miner.instance.onStatsChange += OnMinerStatsChange;

      UpdateRunningState();
    }
    #endregion

    #region Events
    void OnMinerStartOrStop()
    {
      UpdateRunningState();
    }

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

    void OnMonitorValueUpdated()
    {
      OnPropertyChanged(nameof(cpuUsageForMining));
      OnPropertyChanged(nameof(cpuUsageOverall));
    }

    void OnMinerStatsChange()
    {
      UpdateRunningState();
    }
    #endregion

    #region Write
    void Start(
      bool wasManuallyStarted)
    {
      Miner.instance.Start(wasManuallyStarted);
    }

    void Stop()
    {
      Miner.instance.Stop();
    }

    // TODO Switch to event instead of poll
    void UpdateRunningState()
    {
      if (Miner.instance.isMinerRunning)
      {
        startOrStopText = "Stop";
      }
      else
      {
        startOrStopText = "Force Start";
      }
      OnPropertyChanged(nameof(statsBoxList));
      OnPropertyChanged(nameof(additionalWalletStats));
      OnPropertyChanged(nameof(isCurrentlyMining));
      OnPropertyChanged(nameof(currentlyMiningFor));
      OnPropertyChanged(nameof(cpuUsageForMining));
      OnPropertyChanged(nameof(cpuUsageForMining0To100000));
      OnPropertyChanged(nameof(cpuUsageOverall0to100000));
      OnPropertyChanged(nameof(cpuUsageMiningText));
      OnPropertyChanged(nameof(cpuUsageText));
        }
    #endregion
  }
}
