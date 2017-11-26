using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace HD
{
  /// <summary>
  /// Loaded on app start.
  /// Saved anytime a config changes.
  /// </summary>
  [Serializable]
  [JsonObject(MemberSerialization.OptIn)]
  public class MinerConfig
  {
    #region Data
    static readonly string minerConfigFilename = "config.json";

    [JsonProperty]
    string _workerName = "anonymous";

    [JsonProperty]
    TimeSpan _timeTillIdle = TimeSpan.FromMinutes(10);

    [JsonProperty]
    double _maxCpuWhileIdle = .5;

    [JsonProperty]
    double _maxCpuWhileActive = .2;
    #endregion

    #region Properties
    public bool isCurrentlyIdle
    {
      get
      {
        return IdleTimeFinder.GetIdleTime() >= timeTillIdle;
      }
    }

    public double currentTargetCpu
    {
      get
      {
        if (isCurrentlyIdle)
        {
          return maxCpuWhileIdle;
        }
        else
        {
          return maxCpuWhileActive;
        }
      }
    }

    public double maxCpuWhileIdle
    {
      get
      {
        return _maxCpuWhileIdle;
      }
      set
      {
        _maxCpuWhileIdle = value;
        if (maxCpuWhileActive > maxCpuWhileIdle)
        { // while active should always be less than while idle
          _maxCpuWhileActive = maxCpuWhileIdle;
        }
        Save();
      }
    }

    public double maxCpuWhileActive
    {
      get
      {
        return _maxCpuWhileActive;
      }
      set
      {
        _maxCpuWhileActive = value;
        if (maxCpuWhileActive > maxCpuWhileIdle)
        { // while active should always be less than while idle
          _maxCpuWhileIdle = maxCpuWhileActive;
        }
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

    public TimeSpan timeTillIdle
    {
      get
      {
        return _timeTillIdle;
      }
      set
      {
        ValidateTimeTill(ref value);
        _timeTillIdle = value;
      }
    }

    /// <summary>
    /// Controlled by the maxCPU settings.
    /// Always targets worst case number of threads.
    /// Sleep statements ensure hitting the CPU target.
    /// </summary>
    public int numberOfThreads
    {
      get
      {
        double maxResources = Math.Max(maxCpuWhileActive, maxCpuWhileIdle);
        // TODO
        //Debug.Assert(maxResources > 0);
        //Debug.Assert(maxResources <= 1.001);

        return (int)Math.Ceiling(Environment.ProcessorCount * maxResources);
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