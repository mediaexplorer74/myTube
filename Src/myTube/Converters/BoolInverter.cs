// myTube.BoolInverter


using System;
using Windows.UI.Xaml.Data;

namespace myTube.Converters
{
  public class BoolInverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language) => value is bool flag ? (object) !flag : (object) (value != null);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
