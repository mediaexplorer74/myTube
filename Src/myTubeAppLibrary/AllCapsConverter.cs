// Decompiled with JetBrains decompiler
// Type: myTube.AllCapsConverter
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class AllCapsConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (value == null)
        return (object) null;
      return parameter != null && parameter.ToString() == "lower" ? (object) value.ToString().ToLower() : (object) value.ToString().ToUpper();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => (object) value.ToString();
  }
}
