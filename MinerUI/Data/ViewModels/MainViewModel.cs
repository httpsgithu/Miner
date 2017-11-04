using System;
using System.Collections.ObjectModel;

namespace HD
{
  public class MainViewModel : ViewModel
  {
    #region Data
    const string windowTitleBase = "HardlyDifficult Miner",
       windowTitleIdle = windowTitleBase + " (idle)",
       windowTitleForStream = windowTitleBase + " (mining for the stream)", // TODO names
       windowTitleForYou = windowTitleBase + " (mining for yourself)";

    public ObservableCollection<MiningStatsBoxViewModel> StatsBoxList { get; set; }
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

    public double cpuUsageOverall
    {
      get
      {
        return HardwareMonitor.percentTotalCPU;
      }
      set { } // TODO read only
    }

    public bool shouldStartWithWindows
    {
      get
      {
        return StartWithWindows.shouldStartWithWindows;
      }
      set
      {
        StartWithWindows.shouldStartWithWindows = value;
        OnPropertyChanged(nameof(shouldStartWithWindows));
      }
    }
    #endregion

    #region Init
    public MainViewModel()
    {
      StatsBoxList = new ObservableCollection<MiningStatsBoxViewModel>
      {
        new MiningStatsBoxViewModel(true, StatsBoxUseCase.IntervalEstimatedEarningsFromAllCurrentMiners),
        new MiningStatsBoxViewModel(false, StatsBoxUseCase.IntervalEstimatedEarningsFromMe),
        new MiningStatsBoxViewModel(false, StatsBoxUseCase.TotalContribution)
      };
    }
    #endregion

    #region Public
    public void FastRefresh()
    {
      for (int i = 0; i < StatsBoxList.Count; i++)
      {
        StatsBoxList[i].Refresh();
      }
      HardwareMonitor.RefreshValues();
      OnPropertyChanged(nameof(cpuUsageForMining));
      OnPropertyChanged(nameof(cpuUsageOverall));
    }
    #endregion    
  }
}
