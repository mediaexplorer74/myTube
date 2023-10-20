// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.NullableBoolToBoolConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class NullableBoolToBoolConverter : IValueConverter
  {
    public bool IsReversed { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (this.IsReversed)
        return (object) (bool?) value;
      bool? nullable = value as bool?;
      return (object) (bool) (!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      if (!this.IsReversed)
        return (object) (bool?) value;
      bool? nullable = value as bool?;
      return (object) (bool) (!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0));
    }
  }
}
