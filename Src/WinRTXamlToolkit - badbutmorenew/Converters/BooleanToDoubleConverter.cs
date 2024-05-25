// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.BooleanToDoubleConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class BooleanToDoubleConverter : IValueConverter
  {
    private const double Epsilon = 0.001;

    public double FalseValue { get; set; }

    public double TrueValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language) => (object) ((bool) value ? this.TrueValue : this.FalseValue);

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      double num = (double) value;
      if (Math.Abs(num - this.TrueValue) < 0.001)
        return (object) true;
      if (Math.Abs(num - this.FalseValue) < 0.001)
        return (object) false;
      throw new ArgumentException(string.Format("BooleanToDoubleConverter configured to convert FalseValue={0} or TrueValue={1}. Value passed to convert back: {2}", (object) this.FalseValue, (object) this.TrueValue, value), nameof (value));
    }
  }
}
