using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HD
{
  public class Beneficiaries
  {
    #region Data
    readonly List<Beneficiary> beneficiaryList = new List<Beneficiary>();

    // TODO does this work without the path?
    static readonly string beneficiaryFilename = Path.Combine(
      Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "beneficiaries.json");

    static readonly BeneficiarySorter beneficiarySorter = new BeneficiarySorter();

    static readonly Random random = new Random();

    const string devName = "HardlyDifficult";

    const string devWallet = "14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE";

    const double devMinPercent = .01;

    /// <summary>
    /// Should == 1, but cached here to adjust numbers when <> 1.
    /// Recalibrates on Save();
    /// </summary>
    double totalPercentContribution;
    #endregion

    #region Properties
    /// <summary>
    /// Sum from each beneficiary with > 0% support.
    /// </summary>
    public double totalWorkerHashRateMHpS
    {
      get
      {
        double totalHashRate = 0;
        for (int i = 0; i < beneficiaryList.Count; i++)
        {
          Beneficiary beneficiary = beneficiaryList[i];
          if (beneficiary.percentTime > 0)
          {
            totalHashRate += beneficiaryList[i].apiWorkerList?.totalWorkerHashRateMHpS ?? 0;
          }
        }
        return totalHashRate;
      }
    }
    public double totalContributionInBitcoin
    {
      get
      {
        double totalContribution = 0;
        for (int i = 0; i < beneficiaryList.Count; i++)
        {
          Beneficiary beneficiary = beneficiaryList[i];
          totalContribution += beneficiaryList[i].totalMinedInBitcoin;
        }
        return totalContribution;
      }
    }

    #endregion

    #region Init
    public Beneficiaries()
    {
      try
      {
        if (File.Exists(beneficiaryFilename))
        {
          string beneficiariesJson = File.ReadAllText(beneficiaryFilename);
          if (string.IsNullOrEmpty(beneficiariesJson) == false)
          {
            List<Beneficiary> existingList = JsonConvert.DeserializeObject<List<Beneficiary>>(beneficiariesJson);
            if (existingList != null)
            {
              for (int i = 0; i < existingList.Count; i++)
              {
                AddBeneficiary(existingList[i], true);
              }
            }
          }
        }
        Save();
      }
      catch (Exception e)
      {
        Log.ParsingError(nameof(Beneficiaries), nameof(Beneficiaries), e);
      }
    }
    #endregion

    #region Event
    public void Refresh()
    {
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        Beneficiary beneficiary = beneficiaryList[i];
        beneficiary.apiWorkerList?.BeginRead();
      }
    }
    #endregion

    #region Public
    public void Save()
    {
      // TODO confirm dev requirements
      bool found = false;
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        Beneficiary beneficiary = beneficiaryList[i];
        if (beneficiary.wallet == devWallet)
        {
          beneficiary.percentTime = Math.Max(devMinPercent, beneficiary.percentTime);
          found = true;
          break;
        }
      }
      if (found == false)
      {
        AddBeneficiary(new Beneficiary(devName, devWallet, devMinPercent), true);
      }

      if (totalPercentContribution > 0 && Math.Abs(1 - totalPercentContribution) > .01)
      {
        for (int i = 0; i < beneficiaryList.Count; i++)
        {
          beneficiaryList[i].percentTime = beneficiaryList[i].percentTime / totalPercentContribution;
        }
      }
      totalPercentContribution = CalcTotalPercent();
      Debug.Assert(beneficiaryList.Count == 0 || Math.Abs(totalPercentContribution - 1) < .01);

      string beneficiaryJson = JsonConvert.SerializeObject(beneficiaryList);
      File.WriteAllText(beneficiaryFilename, beneficiaryJson);
    }

    public void AddBeneficiary(
      Beneficiary beneficiaryToAdd,
      bool shouldSkipSave = false)
    {
      // De-dupe
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        Beneficiary beneficiary = beneficiaryList[i];
        if (beneficiary.wallet == beneficiaryToAdd.wallet)
        {
          RemoveBeneficiary(beneficiary);
          break;
        }
      }

      beneficiaryList.Add(beneficiaryToAdd);
      beneficiaryList.Sort(beneficiarySorter);
      if (shouldSkipSave == false)
      {
        Save();
      }

      totalPercentContribution = CalcTotalPercent();
    }

    public void RemoveBeneficiary(
      Beneficiary beneficiary)
    {
      beneficiaryList.Remove(beneficiary);
      Save();
    }

    public Beneficiary PickAWinner()
    {
      double rngValue = random.NextDouble() * totalPercentContribution;
      Beneficiary winner = beneficiaryList[0];
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        if (rngValue <= beneficiaryList[i].percentTime)
        {
          winner = beneficiaryList[i];
        }
        else
        {
          rngValue -= beneficiaryList[i].percentTime;
        }
      }

      return winner;
    }
    #endregion

    #region Helpers
    double CalcTotalPercent()
    {
      double totalPercent = 0;
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        if (beneficiaryList[i].isValidAndActive)
        {
          totalPercent += beneficiaryList[i].percentTime;
        }
      }

      return totalPercent;
    }
    #endregion
  }
}
