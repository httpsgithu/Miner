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
        AutoUpdater.Start("https://www.HardlyDifficult.com/Miner/AutoUpdater.xml");
      };
      timer.Start();
    }

    void AutoUpdaterOnCheckForUpdateEvent(
      UpdateInfoEventArgs args)
    {
      if (args != null && args.IsUpdateAvailable)
      {
        try
        {
          if (AutoUpdater.DownloadUpdate())
          {
            Environment.Exit(0); // or app.current.quit?
          }
        }
        catch { }
      }
    }
  }
}
