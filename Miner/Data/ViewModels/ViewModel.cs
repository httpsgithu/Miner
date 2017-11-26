using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HD
{
  public abstract class ViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) //[CallerMemberName] allows you to call this method without having to send the name. less typing. 
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
