using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HD
{
  /// <summary>
  /// Holds the collection of wallets to mine for, with convenience functions for working with the collection.
  /// </summary>
  public class Beneficiaries
  {
    #region Constants
    const string devName = "HardlyDifficult";

    const string devWallet = "14VzFa1eQjcmHp7i3tSTCK3TcWP8kHWhLE";

    const double devMinPercent = .01;

    static readonly string beneficiaryFilename = Path.Combine(
      Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "beneficiaries.json");

    static readonly BeneficiarySorter beneficiarySorter = new BeneficiarySorter();
    #endregion

    #region Data
    public event Action onBeneficiaryListChanged;

    readonly List<Beneficiary> beneficiaryList = new List<Beneficiary>();

    static readonly Random random = new Random();

    /// <summary>
    /// Should == 1, but cached here to adjust numbers when <> 1.
    /// Recalibrates on Save();
    /// </summary>
    double totalPercentContribution;
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
      }
      catch (Exception e)
      {
        Log.ParsingError(nameof(Beneficiaries), nameof(Beneficiaries), e);
      }

      Save();
    }
    #endregion

    #region Public
    public void AddBeneficiary(
      Beneficiary beneficiaryToAdd,
      bool shouldSkipSave = false)
    {
      Debug.Assert(beneficiaryToAdd != null);

      // De-dupe
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        Beneficiary beneficiary = beneficiaryList[i];
        if (beneficiary.wallet == beneficiaryToAdd.wallet)
        {
          beneficiaryToAdd.percentTime = Math.Max(beneficiaryToAdd.percentTime, beneficiary.percentTime);
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
      onBeneficiaryListChanged?.Invoke();
    }

    public void RemoveBeneficiary(
      Beneficiary beneficiary)
    {
      Debug.Assert(beneficiary != null);

      if (beneficiaryList.Remove(beneficiary))
      {
        Save();
        onBeneficiaryListChanged?.Invoke();
      }
    }

    public void Save()
    {
      bool foundDevWallet = false;
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        Beneficiary beneficiary = beneficiaryList[i];
        if (beneficiary.wallet == devWallet)
        {
          beneficiary.percentTime = Math.Max(devMinPercent, beneficiary.percentTime);
          foundDevWallet = true;
          break;
        }
      }
      if (foundDevWallet == false)
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
      Debug.Assert(beneficiaryList.Count > 0);
      Debug.Assert(Math.Abs(totalPercentContribution - 1) < .01);

      string beneficiaryJson = JsonConvert.SerializeObject(beneficiaryList);
      Debug.Assert(string.IsNullOrWhiteSpace(beneficiaryJson) == false);

      File.WriteAllText(beneficiaryFilename, beneficiaryJson);
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
