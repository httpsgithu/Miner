using JobManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace HD
{
  public class Miner
  {
    #region Data
    public static readonly Miner instance = new Miner();

    public readonly Settings settings = new Settings();

    public static bool isFirstLaunch;

    public event Action onHashRateUpdate;

    public MiningAlgorithm currentMiner
    {
      get; private set;
    }

    static readonly TimeSpan minTimeBetweenStarts = TimeSpan.FromMinutes(5);

    DateTime lastConnectionTime;

    public bool wasManuallyStarted
    {
      get; private set;
    }
    #endregion

    #region Properties
    public bool isMachineIdle
    {
      get
      {
        TimeSpan idleTime = IdleTimeFinder.GetIdleTime();
        return idleTime > settings.minerConfig.timeTillAutoRun;
      }
    }
    #endregion

    #region Events
    internal static void OnFirstLaunch()
    {
      isFirstLaunch = true;
    }

    public void OnTick()
    {
      settings.RefreshNetworkAPIsIfCooldown();
      settings.beneficiaries.Refresh();
    }

    internal void OnHashRateUpdate()
    {
      onHashRateUpdate?.Invoke();
    }

    internal void OnMinerResultsAccepted()
    {
      if (DateTime.Now - lastConnectionTime < TimeSpan.FromMinutes(5))
      { // Don't switch wallets unless it's been at least 5 minutes.
        return;
      }

      // Starting again will pick a new winner 
      // If no change, this is a noop
      Start(wasManuallyStarted);
    }
    #endregion

    #region Public
    public void Start(
      bool wasManuallyStarted)
    {
      if (wasManuallyStarted == false)
      {
        if (DateTime.Now - lastConnectionTime < minTimeBetweenStarts)
        { // Too soon to auto connect (blocks changing wallets/algorithms)
          return;
        }
      }
      else
      {
        if (currentMiner != null && DateTime.Now - lastConnectionTime < minTimeBetweenStarts)
        { // If running, this is a change wallet request.  Ignore b/c too soon
          return;
        }
      }

      Beneficiary winner = settings.beneficiaries.PickAWinner();
      Start(winner, wasManuallyStarted);
    }

    public void Stop()
    {
      currentMiner?.Close();
      currentMiner = null;
    }
    #endregion

    #region Helpers
    void Start(
      Beneficiary winner,
      bool wasManuallyStarted)
    {
      lastConnectionTime = DateTime.Now;
      this.wasManuallyStarted = wasManuallyStarted;

      if (currentMiner != null && currentMiner.currentBeneficiary == winner)
      { // No change
        return;
      }

      currentMiner?.Close();

      // This is where we select the most profitable algorithm...
      // once we support more than one.
      currentMiner = new CryptoNightMiner(winner, wasManuallyStarted);
    }
    #endregion
  }
}
