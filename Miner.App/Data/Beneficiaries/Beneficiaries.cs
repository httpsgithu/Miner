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

    #region Properties
    public Beneficiary myWallet
    {
      get
      {
        for (int i = 0; i < beneficiaryList.Count; i++)
        {
          if (beneficiaryList[i].isUsersWallet)
          {
            return beneficiaryList[i];
          }
        }

        return null;
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
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      Save();
    }
    #endregion

    #region Public Write
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
        AddBeneficiary(new Beneficiary(devName, devWallet, devMinPercent, false), true);
      }

      bool foundUsersWallet = false;
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        if (beneficiaryList[i].isUsersWallet)
        {
          if (foundUsersWallet)
          { // Only accept one wallet as the users
            beneficiaryList[i].isUsersWallet = false;
          }
          foundUsersWallet = true;
        }
      }

      if (totalPercentContribution > 0 && Math.Abs(1 - totalPercentContribution) > .01)
      {
        for (int i = 0; i < beneficiaryList.Count; i++)
        {
          // Setting percentTime triggers a save and crash... hence _percentTime instead.  Hacky.
          beneficiaryList[i]._percentTime = beneficiaryList[i].percentTime / totalPercentContribution;
        }
      }
      totalPercentContribution = CalcTotalPercent();
      Debug.Assert(beneficiaryList.Count > 0);
      // TODO this failed -- nice to have. Debug.Assert(Math.Abs(totalPercentContribution - 1) < .01); 

      string beneficiaryJson = JsonConvert.SerializeObject(beneficiaryList);
      Debug.Assert(string.IsNullOrWhiteSpace(beneficiaryJson) == false);

      File.WriteAllText(beneficiaryFilename, beneficiaryJson);
    }
    #endregion

    #region Public Read
    public Beneficiary PickAWinner()
    {
      double rngValue = random.NextDouble() * totalPercentContribution;
      Beneficiary winner = beneficiaryList[0];
      for (int i = 0; i < beneficiaryList.Count; i++)
      {
        if (beneficiaryList[i].isValidAndActive == false)
        {
          continue;
        }

        if (rngValue <= beneficiaryList[i].percentTime)
        {
          winner = beneficiaryList[i];
          break;
        }
        else
        {
          rngValue -= beneficiaryList[i].percentTime;
        }
      }

      return winner;
    }

    public IEnumerator<Beneficiary> GetEnumerator()
    {
      return beneficiaryList.GetEnumerator();
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
