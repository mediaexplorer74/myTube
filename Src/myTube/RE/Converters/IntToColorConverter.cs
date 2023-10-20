// Decompiled with JetBrains decompiler
// Type: myTube.Converters.IntToColorConverter
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace myTube.Converters
{
  public class IntToColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (!(value is int num))
        return (object) new Color();
      Color color = Color.FromArgb((byte) (num >> 24 & (int) byte.MaxValue), (byte) (num >> 16 & (int) byte.MaxValue), (byte) (num >> 8 & (int) byte.MaxValue), (byte) (num & (int) byte.MaxValue));
      if (color.A == (byte) 0)
        color.A = byte.MaxValue;
      return (object) color;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
