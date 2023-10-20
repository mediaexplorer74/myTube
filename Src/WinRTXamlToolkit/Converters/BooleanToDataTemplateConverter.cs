// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.BooleanToDataTemplateConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class BooleanToDataTemplateConverter : DependencyObject, IValueConverter
  {
    public DataTemplate FalseTemplate { get; set; }

    public DataTemplate TrueTemplate { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language) => !(bool) value ? (object) this.FalseTemplate : (object) this.TrueTemplate;

    public object ConvertBack(object value, Type targetType, object parameter, string language) => (object) (value != this.FalseTemplate);
  }
}
