// Decompiled with JetBrains decompiler
// Type: myTube.BoolToVisibility
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

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

                    //RnD
        //case bool? _:
        //  bool? nullable = value as bool?;
        //  return nullable.HasValue ? (object) (Visibility) (nullable.Value ? 0 : 1) : (object) (Visibility) 1;
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
