using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HD
{
  /// <summary>
  /// Base class for every view model.  This will deal with threading issues.
  /// </summary>
  public abstract class ViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    static SynchronizationContext context;

    public ViewModel()
    {
        if(context == null && SynchronizationContext.Current != null)
            context = SynchronizationContext.Current;
    }

    /// <summary>
    /// This may be called by any thread.
    /// 
    /// Usage:
    /// OnPropertyChanged() when I changed
    /// OnPropertyChanged("fieldName") others changed
    /// </summary>
    protected void OnPropertyChanged(
      [CallerMemberName] string propertyName = null)
    {
      if (context != null)
      {
        context.Send((state) =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }, null);
      }
    }
  }
}