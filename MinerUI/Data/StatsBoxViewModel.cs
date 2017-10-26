using System;
using System.Windows;

namespace HD
{
  public class StatsBoxViewModel : ViewModel 
  {
    #region Data
    string _backgroundColor;
    string _title;
    string _leftValue;
    string _rightValue;
    Visibility _rightValueVisibility;
    #endregion

    #region Properties
    public string Title
    {
      get { return _title; }
      set
      {
        _title = value;
        OnPropertyChanged(nameof(Title));
      }
    }

    public string LeftValue
    {
      get { return _leftValue; }
      set
      {
        _leftValue = value;
        OnPropertyChanged(nameof(LeftValue));
      }
    }

    public string RightValue
    {
      get { return _rightValue; }
      set
      {
        _rightValue = value;
        OnPropertyChanged(nameof(RightValue));
      }
    }

    public string BackgroundColor
    {
      get
      {
        return _backgroundColor;
      }
      set
      {
        _backgroundColor = value;
        OnPropertyChanged(nameof(BackgroundColor));
      }
    }

    public Visibility RightValueVisibility
    {
      get { return _rightValueVisibility; }
      set
      {
        _rightValueVisibility = value;
        OnPropertyChanged(nameof(RightValueVisibility));
      }
    }
    #endregion
  }
}
