using System;
using System.Collections.Generic;
using System.Reflection;

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
        OnPropertyChanged(nameof(maxCpuWhileActive));
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
        OnPropertyChanged(nameof(maxCpuWhileIdle));
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

    public int timeTillIdleInMinutes
    {
      get
      {
        return (int)Math.Round(timeTillIdle.TotalMinutes);
      }
      set
      {
        timeTillIdle = TimeSpan.FromMinutes(value);
      }
    }

    public string timeTillIdleInMinutesString
    {
      get
      {
        if (timeTillIdleInMinutes == 1)
        {
          return "1 min";
        }
        else
        {
          return $"{timeTillIdleInMinutes} mins";
        }
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
        OnPropertyChanged(nameof(timeTillIdleInMinutes));
        OnPropertyChanged(nameof(timeTillIdleInMinutesString));
      }
    }

    public string myWallet
    {
      get
      {
        Beneficiary beneficiary = Miner.instance.settings.beneficiaries.myWallet;
        return beneficiary?.wallet;
      }
      set
      {
        Beneficiary beneficiary = Miner.instance.settings.beneficiaries.myWallet;
        if (beneficiary != null)
        {
          if (beneficiary.wallet == value)
          { // No change
            return;
          }
          // Remove old, create a new 
          Miner.instance.settings.beneficiaries.RemoveBeneficiary(beneficiary);
          beneficiary = new Beneficiary(beneficiary.name, value, beneficiary.percentTime, true);
        }
        else
        {
          beneficiary = new Beneficiary("My Wallet", value, 1, true);
        }
        Miner.instance.settings.beneficiaries.AddBeneficiary(beneficiary);

        if(beneficiary.percentTime <= 0)
        {
          beneficiary.percentTime = .01;
        }

        if(beneficiary.isValidAndActive == false)
        {
          throw new Exception("Not a valid BitCoin wallet ID");
        }
      }
    }

    public string version
    {
      get
      {
        Assembly assembly = Assembly.GetEntryAssembly();
        return assembly.GetName().Version.ToString();
      }
    }

    public Period period
    {
      get
      {
        return Miner.instance.settings.minerConfig.period;
      }
      set
      {
        Miner.instance.settings.minerConfig.period = value;
        OnPropertyChanged();
      }
    }

    public IEnumerable<Period> periodTypeList
    {
      get
      {
        Array list = Enum.GetValues(typeof(Period));
        for (int i = 0; i < list.Length; i++)
        {
          yield return (Period)list.GetValue(i);
        }
      }
    }

    public Currencies currency
    {
        get
        {
            return Miner.instance.settings.minerConfig.currency;
        }
        set
        {
            Miner.instance.settings.minerConfig.currency = value;
            OnPropertyChanged();
        }
        }

    public IEnumerable<Currencies> currencyList
    {
        get
        {
            Array list = Enum.GetValues(typeof(Currencies));
            for (int i = 0; i < list.Length; i++)
            {
                yield return (Currencies)list.GetValue(i);
            }
        }
    }
  }
}
