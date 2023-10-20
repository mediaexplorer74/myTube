// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.NullableBoolToVisibilityConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class NullableBoolToVisibilityConverter : IValueConverter
  {
    public bool IsReversed { get; set; }

    public bool TrueIsVisible { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (this.IsReversed)
      {
        Visibility visibility = (Visibility) value;
        return (object) new bool?(visibility == null && this.TrueIsVisible || visibility == 1 && !this.TrueIsVisible);
      }
      bool? nullable = value as bool?;
      return (object) (Visibility) (!nullable.HasValue || !nullable.Value ? (!this.TrueIsVisible ? 0 : 1) : (this.TrueIsVisible ? 0 : 1));
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
      if (this.IsReversed)
      {
        bool? nullable = value as bool?;
        return (object) (Visibility) (!nullable.HasValue || !nullable.Value ? (!this.TrueIsVisible ? 0 : 1) : (this.TrueIsVisible ? 0 : 1));
      }
      Visibility visibility = (Visibility) value;
      return (object) new bool?(visibility == null && this.TrueIsVisible || visibility == 1 && !this.TrueIsVisible);
    }
  }
}
