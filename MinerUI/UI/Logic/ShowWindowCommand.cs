using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;

namespace HD.Commands
{
  public class ShowSampleWindowCommand : MarkupExtension, ICommand
  {
    static ShowSampleWindowCommand instance;

    public event EventHandler CanExecuteChanged;

    public ShowSampleWindowCommand()
    {
      instance = this;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return instance;
    }

    bool ICommand.CanExecute(object parameter)
    {
      return true;
    }

    void ICommand.Execute(object parameter)
    {
      MainWindow.instance.Visibility = Visibility.Visible;
      Win32.Unminimize(MainWindow.instance);
    }
  }

  public static class Win32
  {
    public static void Unminimize(Window window)
    {
      var hwnd = (HwndSource.FromVisual(window) as HwndSource).Handle;
      ShowWindow(hwnd, ShowWindowCommands.Restore);
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

    private enum ShowWindowCommands : int
    {
      /// <summary>
      /// Activates and displays the window. If the window is minimized or 
      /// maximized, the system restores it to its original size and position. 
      /// An application should specify this flag when restoring a minimized window.
      /// </summary>
      Restore = 9,
    }
  }
}
