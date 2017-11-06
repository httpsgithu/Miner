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

    readonly DispatcherTimer timer = new DispatcherTimer();
    #endregion

    #region Init
    public MainWindow()
    {
      instance = this;
      InitializeComponent();
      mainWindow.DataContext = new MainViewModel();

      timer.Interval = TimeSpan.FromMilliseconds(200);
      timer.Tick += OnTick;
      timer.Start();

      maxNumberOfThreads.Content = Environment.ProcessorCount;
      sliderNumberOfThreads.Maximum = Environment.ProcessorCount;
      if (Miner.isFirstLaunch)
      {
        ((MainViewModel)DataContext).shouldStartWithWindows = true;
      }
      sliderPercentToHD.Value = 0.2;
    }

    void OnWindowClosing(
      object sender,
      CancelEventArgs e)
    {
      Miner.instance.settings.SaveOnExit();
      timer.Tick -= OnTick;
      timer.Stop();
      Stop();
    }
    #endregion

    #region Events
    void OnTick(
      object sender,
      EventArgs e)
    {
      ((MainViewModel)DataContext).FastRefresh();
      Miner.instance.OnTick();
    }

    void OnStartStopButtonClick(
      object sender,
      RoutedEventArgs e)
    {
      if (Miner.instance.currentMiner != null)
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
      if (Miner.instance.currentMiner != null)
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
  }
}
