// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.MultiplyConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class MultiplyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      double num = System.Convert.ToDouble(value);
      return (object) (System.Convert.ToDouble(parameter) * num);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      double num = System.Convert.ToDouble(value);
      return (object) (System.Convert.ToDouble(parameter) / num);
    }
  }
}
