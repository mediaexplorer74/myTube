// myTube.LowerStringFormatConverter

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class LowerStringFormatConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (parameter is string str1)
      {
        char[] chArray = new char[1]{ '/' };
        string[] strArray = str1.Split(chArray);
        string str = App.Strings[strArray[0]];
        if (str != null)
          return (object) str.ToLower();
        if (strArray.Length != 0)
          return (object) strArray[1];
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) 
            => throw new NotImplementedException();
  }
}
