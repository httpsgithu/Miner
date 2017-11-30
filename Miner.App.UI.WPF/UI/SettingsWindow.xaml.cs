using System;
using MahApps.Metro.Controls;

namespace HD
{
  public partial class SettingsWindow : MetroWindow
  {
    #region Data
    public static SettingsWindow instance;
    #endregion

    #region Init
    public SettingsWindow(
      SettingsViewModel settingsViewModel)
    {
      instance = this;
      InitializeComponent();
      DataContext = settingsViewModel;
    }
    #endregion
  }
}
