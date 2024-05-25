// Decompiled with JetBrains decompiler
// Type: myTube.Converters.SubtitlePlacementToVerticalAlignment
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace myTube.Converters
{
  public class SubtitlePlacementToVerticalAlignment : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (!(value is SubtitlePlacement subtitlePlacement))
        return (object) (VerticalAlignment) 2;
      if (subtitlePlacement == SubtitlePlacement.Bottom)
        return (object) (VerticalAlignment) 2;
      return subtitlePlacement == SubtitlePlacement.Top ? (object) (VerticalAlignment) 0 : (object) (VerticalAlignment) 2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
