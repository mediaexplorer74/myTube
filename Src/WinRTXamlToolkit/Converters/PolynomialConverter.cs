// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.PolynomialConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Converters
{
  public class PolynomialConverter : IValueConverter
  {
    public DoubleCollection Coefficients { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      double x = (double) value;
      double num = 0.0;
      for (int index = ((ICollection<double>) this.Coefficients).Count - 1; index >= 0; --index)
        num += ((IList<double>) this.Coefficients)[index] * Math.Pow(x, (double) (((ICollection<double>) this.Coefficients).Count - 1 - index));
      return (object) num;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
  }
}
