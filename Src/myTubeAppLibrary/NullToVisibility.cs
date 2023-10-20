// Decompiled with JetBrains decompiler
// Type: myTube.NullToVisibility
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

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
