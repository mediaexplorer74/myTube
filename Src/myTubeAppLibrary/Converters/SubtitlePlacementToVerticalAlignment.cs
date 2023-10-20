// Decompiled with JetBrains decompiler
// Type: myTube.Converters.SubtitlePlacementToVerticalAlignment
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

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
