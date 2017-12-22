using System;
using MahApps.Metro.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

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


    void OnHyperlinkClick_Coinbase(
      object sender, 
      RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
  }
}
