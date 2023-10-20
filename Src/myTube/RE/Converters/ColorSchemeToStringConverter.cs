// Decompiled with JetBrains decompiler
// Type: myTube.Converters.ColorSchemeToStringConverter
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using Windows.UI.Xaml.Data;

namespace myTube.Converters
{
  public class ColorSchemeToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (!(value is ColorSchemes colorSchemes))
        return value;
      switch (colorSchemes)
      {
        case ColorSchemes.Default:
          return (object) App.Strings["settings.colorschemes.default", "default"].ToLower();
        case ColorSchemes.Accent:
          return (object) App.Strings["settings.colorschemes.defaultaccent", "default (accent color)"].ToLower();
        case ColorSchemes.Classic:
          return (object) App.Strings["settings.colorschemes.classic", "classic"].ToLower();
        case ColorSchemes.YouTube:
          return (object) "YouTube";
        default:
          return (object) colorSchemes.ToString().ToLower();
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
