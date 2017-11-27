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
      context = new SynchronizationContext();
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) //[CallerMemberName] allows you to call this method without having to send the name. less typing. 
    {
      context.Send((state) =>
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }, null);
    }
  }
}
