using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace HD
{
  /// <summary>
  /// Holds a collection of the saved and cached settings.
  /// </summary>
  public class Settings
  {
    #region Data
    // Saved Info
    public readonly MinerConfig minerConfig = MinerConfig.LoadOrCreate();
    public readonly Beneficiaries beneficiaries = new Beneficiaries();
    #endregion

    #region Public
    public void SaveOnExit()
    {
      // Saves latest worker stats
      beneficiaries.Save();

      // Don't need to save the config, it saves when values change
    }
    #endregion
  }
}
