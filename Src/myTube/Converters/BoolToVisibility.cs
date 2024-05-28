// myTube.BoolToVisibility

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class BoolToVisibility : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if ((string) parameter == "visibleOnWindowsOnly" && TileHelper.Platform != PlatformType.Windows)
        return (object) (Visibility) 1;
      switch (value)
      {
        case bool flag:
          return (object) (Visibility) (flag ? 0 : 1);

        case int num:
          return (object) (Visibility) (num > 0 ? 0 : 1);
                    
              // C# 9 +
        //case bool? _:
       //
                    bool? nullable = value as bool?;
       //   return nullable.HasValue ? (object) (Visibility) (nullable.Value ? 0 : 1) : (object) (Visibility) 1;
       
        default:
          return (object) (Visibility) 0;
      }
    }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
