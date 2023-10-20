// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.BooleanToVisibilityConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public sealed class BooleanToVisibilityConverter : IValueConverter
  {
    public bool IsReversed { get; set; }

    public bool IsInversed { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language) => this.IsReversed ? (object) (bool) (!(value is Visibility ^ this.IsInversed) ? 0 : ((Visibility) value == 0 ? 1 : 0)) : (object) (Visibility) (((!(value is bool flag) ? 0 : (flag ? 1 : 0)) ^ (this.IsInversed ? 1 : 0)) != 0 ? 0 : 1);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => this.IsReversed ? (object) (Visibility) (((!(value is bool flag) ? 0 : (flag ? 1 : 0)) ^ (this.IsInversed ? 1 : 0)) != 0 ? 0 : 1) : (object) (bool) ((!(value is Visibility) ? 0 : ((Visibility) value == 0 ? 1 : 0)) ^ (this.IsInversed ? 1 : 0));
  }
}
