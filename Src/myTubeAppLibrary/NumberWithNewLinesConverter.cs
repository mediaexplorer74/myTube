// Decompiled with JetBrains decompiler
// Type: myTube.NumberWithNewLinesConverter
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class NumberWithNewLinesConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      string str1 = value.ToString();
      string str2 = "";
      string newLine = Environment.NewLine;
      if (parameter != null)
        newLine = parameter.ToString();
      for (int index = 0; index < str1.Length; ++index)
      {
        int num = (str1.Length - index) % 3;
        if (index != str1.Length - 1 && index != 0 && num == 0)
          str2 += newLine;
        str2 += str1[index].ToString();
      }
      return (object) str2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
