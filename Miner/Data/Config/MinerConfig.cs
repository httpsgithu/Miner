using Newtonsoft.Json;
using System;
using System.IO;

namespace HD
{
  /// <summary>
  /// Loaded on app start.
  /// Saved anytime a config changes.
  /// </summary>
  [Serializable]
  public class MinerConfig
  {
    #region Data
    // TODO without path?
    static readonly string minerConfigFilename = System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0, System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\")) + "\\config.json"; 

    int _numberOfThreadsWhenActive = 1;

    double _stopIfCPUGreaterThanWhileActive = 50;

    int _numberOfThreadsWhenIdle = 3;

    double _stopIfCPUGreaterThanWhileIdle = 80;

    string _workerName = "anonymous";

    bool _shouldAutoRun = true;

    TimeSpan _minutesTillAutoRun = TimeSpan.FromMinutes(10);
    #endregion

    #region Properties
    public int numberOfThreadsWhenActive
    {
      get
      {
        return _numberOfThreadsWhenActive;
      }
      set
      {
        ValidateThreadCount(ref value);
        _numberOfThreadsWhenActive = value;
        Save();
      }
    }

    public double stopIfCPUGreaterThanWhileActive
    {
      get
      {
        return _stopIfCPUGreaterThanWhileActive;
      }
      set
      {
        ValidateCPUThreshold(ref value, numberOfThreadsWhenActive);
        _stopIfCPUGreaterThanWhileActive = value;
        Save();
      }
    }

    public int numberOfThreadsWhenIdle
    {
      get
      {
        return _numberOfThreadsWhenIdle;
      }
      set
      {
        ValidateThreadCount(ref value);
        _numberOfThreadsWhenIdle = value;
        Save();
      }
    }

    public double stopIfCPUGreaterThanWhileIdle
    {
      get
      {
        return _stopIfCPUGreaterThanWhileIdle;
      }
      set
      {
        ValidateCPUThreshold(ref value, numberOfThreadsWhenIdle);
        _stopIfCPUGreaterThanWhileIdle = value;
        Save();
      }
    }

    public string workerName
    {
      get
      {
        return _workerName;
      }
      set
      {
        ValidateWorkerName(ref value);
        _workerName = value;
        Save();
      }
    }

    public bool shouldAutoRun
    {
      get
      {
        return _shouldAutoRun;
      }
      set
      {
        _shouldAutoRun = value;
        Save();
      }
    }

    public TimeSpan timeTillAutoRun
    {
      get
      {
        return _minutesTillAutoRun;
      }
      set
      {
        ValidateTimeTill(ref value);
        _minutesTillAutoRun = value;
      }
    }
    #endregion

    #region Init
    private MinerConfig() { }

    public static MinerConfig LoadOrCreate()
    {
      try
      {
        string configText = File.ReadAllText(minerConfigFilename);
        if (string.IsNullOrEmpty(configText) == false)
        {
          return JsonConvert.DeserializeObject<MinerConfig>(configText);
        }
      }
      catch { }
      MinerConfig config = new MinerConfig();
      Miner.OnFirstLaunch();
      config.Save();
      return config;
    }
    #endregion

    #region Helpers
    void Save()
    {
      string config = JsonConvert.SerializeObject(this);
      File.WriteAllText(minerConfigFilename, config);
    }
    #endregion

    #region Validators
    void ValidateThreadCount(
      ref int value)
    {
      if (value < 1)
      {
        value = 1;
      }
      else if (value > Environment.ProcessorCount)
      {
        value = Environment.ProcessorCount;
      }
    }

    void ValidateCPUThreshold(
      ref double value,
      int numberOfThreads)
    {
      double expectedLoad = (double)numberOfThreads / Environment.ProcessorCount;
      expectedLoad *= 1.2; // CPU usage may be greater than one core b/c of hyperthreading
      expectedLoad += 10; // Assume the computer does other things
      if (value < expectedLoad)
      {
        value = expectedLoad;
      }
      if (value > 100)
      {
        value = 100;
      }
    }

    void ValidateWorkerName(
      ref string value)
    {
      if (value == null)
      {
        value = "";
      }

      value = value.Trim();
      if (value.Length > 15)
      {
        value = value.Substring(0, 15);
      }
    }

    void ValidateTimeTill(
      ref TimeSpan value)
    {
      if (value <= TimeSpan.Zero)
      {
        value = TimeSpan.FromMinutes(1);
      }
    }
    #endregion
  }
}