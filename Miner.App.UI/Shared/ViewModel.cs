using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HD
{
  /// <summary>
  /// Every ViewModel should derive from this.
  /// It will forward events to the correct dispatcher.
  /// </summary>
  public abstract class ViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged(
      [CallerMemberName] string propertyName = null) 
    {
      MinerUI.instance.Dispatch(() =>
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      });
    }
  }
}
