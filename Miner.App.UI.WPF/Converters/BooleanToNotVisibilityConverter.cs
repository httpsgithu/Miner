using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HD
{
  public class BooleanToNotVisibilityConverter : IValueConverter
  {
    object IValueConverter.Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if ((bool)value)
      {
        return Visibility.Collapsed;
      }
      else
      {
        return Visibility.Visible;
      }
    }

    object IValueConverter.ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      // TODO what do you want from me?!
      if ((bool)value)
      {
        return Visibility.Collapsed;
      }
      else
      {
        return Visibility.Visible;
      }
    }
  }
}
