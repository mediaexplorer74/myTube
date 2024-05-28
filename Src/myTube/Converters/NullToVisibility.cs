// myTube.NullToVisibility

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class NullToVisibility : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language) => (string) parameter == "inverse" ? (value == null ? (object) (Visibility) 0 : (object) (Visibility) 1) : (value == null ? (object) (Visibility) 1 : (object) (Visibility) 0);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
