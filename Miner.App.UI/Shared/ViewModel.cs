using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HD
{
  public abstract class ViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    readonly SynchronizationContext context;

    public ViewModel()
    {
      context = SynchronizationContext.Current;
    }

    protected void OnPropertyChanged(
      [CallerMemberName] string propertyName = null)
    {
      context.Send((state) =>
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }, null);
    }
  }
}