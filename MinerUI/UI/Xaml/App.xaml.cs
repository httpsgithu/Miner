using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace HD
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    readonly MinerController minerController;

    public App()
    {
      minerController = new MinerController();
    }

    protected override void OnExit(
      ExitEventArgs e)
    {
      base.OnExit(e);

      minerController.Stop();
    }
  }
}
