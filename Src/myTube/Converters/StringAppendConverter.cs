// myTube.StringAppendConverter

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class StringAppendConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language) => (object) parameter.ToString().Replace("*1", value.ToString());

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
