using AutoUpdaterDotNET;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace HD
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    void Application_Startup(
      object sender,
      StartupEventArgs e)
    {
      if (e.Args.Length > 0 && e.Args[0] == Globals.launchedByInstallerArg)
      {
        Process.Start(Assembly.GetExecutingAssembly().Location);
        Application.Current.Shutdown();
        return;
      } 
      AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
      DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
      timer.Tick += delegate
      {
        CheckForUpdates();
      };
      timer.Start();
    }

    static void CheckForUpdates()
    {
      AutoUpdater.Start("https://www.HardlyDifficult.com/Miner/AutoUpdater.xml");
    }

    void AutoUpdaterOnCheckForUpdateEvent(
      UpdateInfoEventArgs args)
    {
      if (args != null && args.IsUpdateAvailable)
      {
        try
        {
          AutoUpdater.DownloadUpdate(onComplete: () => Environment.Exit(0));
        }
        catch { }
      }
    }
  }
}
