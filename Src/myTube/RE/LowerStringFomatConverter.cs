﻿// Decompiled with JetBrains decompiler
// Type: myTube.LowerStringFomatConverter
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class LowerStringFomatConverter : IValueConverter
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

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
