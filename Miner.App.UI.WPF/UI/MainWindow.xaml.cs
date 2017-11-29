using System;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace HD
{
  public partial class MainWindow : MetroWindow
  {
    #region Data
    public static MainWindow instance;
    #endregion

    #region Init
    public MainWindow()
    {
      instance = this;
      InitializeComponent();
      mainWindow.DataContext = new MainViewModel();

      sliderPercentToHD.Value = 0.2;
    }

    void OnWindowClosing(
      object sender,
      CancelEventArgs e)
    {
      Miner.instance.settings.SaveOnExit();
      Stop();
    }
    #endregion

    #region Events
    void OnStartStopButtonClick(
      object sender,
      RoutedEventArgs e)
    {
      if (Miner.instance.isMinerRunning)
      {
        Stop();
      }
      else
      {
        Start(true);
      }
    }
    #endregion

    #region Helpers
    void Start(
      bool wasManuallyStarted)
    {
      Miner.instance.Start(wasManuallyStarted);
      UpdateRunningState();
    }

    void UpdateRunningState()
    {
      
      if (Miner.instance.isMinerRunning)
      {
        StartStopButton.Content = "Stop";
      }
      else
      {
        StartStopButton.Content = "Start";
      }
    }

    void Stop()
    {
      Miner.instance.Stop();
      UpdateRunningState();
    }

    void mainWindow_StateChanged(
      object sender,
      EventArgs e)
    {
      if (mainWindow.WindowState == WindowState.Minimized)
      {
        mainWindow.Visibility = Visibility.Hidden;
      }
    }
    #endregion

    async void reportBug(object sender, RoutedEventArgs e)
    {
      await mainWindow.ShowMessageAsync("ERROR!", "There are no bugs...");
    }

    void miningforTile_Click(object sender, RoutedEventArgs e)
    {
      MiningForFlyout.IsOpen = true;
    }

    void sliderPercentToHD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      labelPercentToHD.Content = ((int)(sliderPercentToHD.Value * 100)) + "%";
    }

    void mainWindow_Loaded(object sender, RoutedEventArgs e)
    {
    }
  }
}
