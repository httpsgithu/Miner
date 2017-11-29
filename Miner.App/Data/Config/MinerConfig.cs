using Miner.OS;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

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
    #region Constants
    static readonly string minerConfigFilename = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

    static readonly TimeSpan minIdleTime = TimeSpan.FromMinutes(1);
    #endregion

    #region Data
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
    public bool shouldStartWithWindows
    {
      get
      {
        return MinerOS.instance.shouldAutoStart;
      }
      set
      {
        MinerOS.instance.shouldAutoStart = value;
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

    /// <summary>
    /// Up to 15 characters.
    /// </summary>
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
        Debug.Assert(maxResources <= 1.001);

        return (int)Math.Max(1, Math.Ceiling(Environment.ProcessorCount * maxResources));
      }
    }
    #endregion

    #region Init
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
      Log.Event("Creating a new default miner config");
      config.shouldStartWithWindows = true;
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
      if (value < minIdleTime)
      {
        value = minIdleTime;
      }
    }
    #endregion
  }
}