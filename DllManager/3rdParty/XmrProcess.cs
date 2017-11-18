//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Text;

//namespace HD
//{
//  public class CryptoNightMiner : MiningAlgorithm
//  {
//    #region Data
//    static readonly string
//      minerConfigFilename = System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0, System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\")) + "\\config.txt",
//      minerOutputFilename = System.Reflection.Assembly.GetEntryAssembly().Location.Substring(0, System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\")) + "\\output.log";

//    readonly FileObserver fileObserver = new FileObserver(minerOutputFilename);
//    #endregion

//    #region Init
//    public CryptoNightMiner(
//      Beneficiary winner,
//      bool wasManuallyStarted)
//      : base(winner)
//    {
//      int numberOfThreads = wasManuallyStarted || Miner.instance.isMachineIdle == false
//        ? Miner.instance.settings.minerConfig.numberOfThreadsWhenActive
//        : Miner.instance.settings.minerConfig.numberOfThreadsWhenIdle;
//      Start(winner.wallet, numberOfThreads);
//    }

//    public override void Close()
//    {
//      base.Close();

//      fileObserver.observers -= OnLog;
//      fileObserver?.Stop();
//    }
//    #endregion

//    #region Events
//    void OnLog(
//      string message)
//    {
//      if (currentBeneficiary == null)
//      { // Not running, how did a log show up?
//        return;
//      }

//      if (message.Contains("Result accepted by the pool."))
//      {
//        Miner.instance.OnMinerResultsAccepted();
//      }
//      else if (message.Contains("Highest:"))
//      {
//        // Highest:  66.3 H/s
//        int indexOfSpace = message.IndexOf(":");
//        if (indexOfSpace < 0)
//        {
//          return;
//        }
//        indexOfSpace++;
//        int indexOfEnd = message.IndexOf("/s");
//        if (indexOfEnd < 0)
//        {
//          return;
//        }

//        int indexOfEndSpace = message.IndexOf(" ", indexOfEnd - 2);
//        string valueString = message.Substring(indexOfSpace, indexOfEndSpace - indexOfSpace);
//        if (double.TryParse(valueString, out currentHashRateMHpS))
//        {
//          string unit = message.Substring(indexOfEndSpace + 1, indexOfEnd - indexOfEndSpace - 1);
//          if (unit.Equals("H", StringComparison.InvariantCulture))
//          {
//            currentHashRateMHpS /= 1000000;
//          }
//          else if (unit.Equals("KH", StringComparison.InvariantCulture))
//          {
//            currentHashRateMHpS /= 1000;
//          }
//          else
//          {
//            Console.WriteLine("Hash rate unit not recognized");
//          }

//          OnHashRateUpdate();
//        }
//      }


//      //Console.WriteLine(message);
//    }
//    #endregion

//    #region Helpers
//    void Start(
//     string wallet,
//     int numberOfThreads)
//    {
//      CreateFileObserver();
//      GenerateMinerConfigFile(wallet, numberOfThreads);

//      process = new Process();
//      process.StartInfo.FileName = $"{Environment.CurrentDirectory}\\xmr-stak-cpu";
//      process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
//      process.StartInfo.UseShellExecute = false;
//      process.StartInfo.LoadUserProfile = false;
//      process.StartInfo.CreateNoWindow = true;
//      process.Start();
//      process.PriorityClass = ProcessPriorityClass.BelowNormal;

//      HardwareMonitor.minerProcessPerformanceCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);

//      // This will close the miner's process if our app crashes
//      windowsJob.AddProcess(process);
//    }

//    void GenerateMinerConfigFile(
//      string wallet,
//      int numberOfThreads)
//    {
//      string json = Xmr.GenerateConfigJson(wallet, numberOfThreads);
//      File.WriteAllText(minerConfigFilename, json);
//    }


//    void CreateFileObserver()
//    {
//      fileObserver.Stop();
//      try
//      {
//        File.Delete(minerOutputFilename);
//      }
//      catch { }
//      fileObserver.observers += OnLog;
//      fileObserver.Start();
//    }
//    #endregion
//  }
//}
