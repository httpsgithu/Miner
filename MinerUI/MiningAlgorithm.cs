using JobManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HD
{
  public abstract class MiningAlgorithm
  {
    public Beneficiary currentBeneficiary
    {
      get; private set;
    }

    public double currentHashRateMHpS;

    protected static readonly WindowsJob windowsJob = new WindowsJob();

    protected Process process;
    DateTime lastUpdate;

    public MiningAlgorithm(
      Beneficiary winner)
    {
      lastUpdate = DateTime.Now;
      this.currentBeneficiary = winner;
    }

    public virtual void Close()
    {
      process?.Kill();
      process?.Close();
      process = null;
      currentBeneficiary = null;
      HardwareMonitor.minerProcessPerformanceCounter = null;
    }

    #region Events
    protected void OnHashRateUpdate()
    {
      double seconds = (DateTime.Now - lastUpdate).TotalSeconds;
      lastUpdate = DateTime.Now;
      currentBeneficiary.totalMinedInBitcoin += currentHashRateMHpS * seconds;
      Miner.instance.OnHashRateUpdate();
    }
    #endregion
  }
}
