using System;

namespace HD
{
  // TODO settings for the beneficiary list
  public class SettingsViewModel : ViewModel
  {
    public bool shouldStartWithWindows
    {
      get
      {
        return Miner.instance.settings.minerConfig.shouldStartWithWindows;
      }
      set
      {
        Miner.instance.settings.minerConfig.shouldStartWithWindows = value;
        OnPropertyChanged();
      }
    }

    public double maxCpuWhileIdle
    {
      get
      {
        return Miner.instance.settings.minerConfig.maxCpuWhileIdle;
      }
      set
      {
        Miner.instance.settings.minerConfig.maxCpuWhileIdle = value;
        OnPropertyChanged();
      }
    }

    public double maxCpuWhileActive
    {
      get
      {
        return Miner.instance.settings.minerConfig.maxCpuWhileActive;
      }
      set
      {
        Miner.instance.settings.minerConfig.maxCpuWhileActive = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Up to 15 characters.
    /// </summary>
    public string workerName
    {
      get
      {
        return Miner.instance.settings.minerConfig.workerName;
      }
      set
      {
        Miner.instance.settings.minerConfig.workerName = value;
        OnPropertyChanged();
      }
    }

    public TimeSpan timeTillIdle
    {
      get
      {
        return Miner.instance.settings.minerConfig.timeTillIdle;
      }
      set
      {
        Miner.instance.settings.minerConfig.timeTillIdle = value;
        OnPropertyChanged();
      }
    }
  }
}
