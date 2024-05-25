// myTube.StringToVisibiltyConverter

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class StringToVisibiltyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (value == null)
        return (object) (Visibility) 1;
      return string.IsNullOrWhiteSpace(value.ToString()) ? (object) (Visibility) 1 : (object) (Visibility) 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
