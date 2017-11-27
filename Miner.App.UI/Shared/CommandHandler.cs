using System;
using System.Windows.Input;

namespace HD
{
  /// <summary>
  /// Added to a button in WPF or ETO to process onClick events.
  /// 
  /// Usage: Button Command="{Binding StartStopCMD}" 
  /// </summary>
  public class CommandHandler : ICommand
  {
    #region Dat
    public event EventHandler CanExecuteChanged;

    readonly Action action;

    readonly bool canExecute;
    #endregion

    #region Init
    public CommandHandler(
      Action action, 
      bool canExecute)
    {
      this.action = action;
      this.canExecute = canExecute;
    }
    #endregion

    #region Write
    public void Execute(
      object parameter)
    {
      action();
    }
    #endregion

    #region Read
    public bool CanExecute(
      object parameter)
    {
      return canExecute;
    }
    #endregion
  }
}
