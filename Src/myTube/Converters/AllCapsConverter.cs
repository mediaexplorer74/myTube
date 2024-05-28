// myTube.AllCapsConverter

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class AllCapsConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (value == null)
        return (object) null;
      return parameter != null && parameter.ToString() == "lower" ? (object) value.ToString().ToLower() : (object) value.ToString().ToUpper();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => (object) value.ToString();
  }
}
