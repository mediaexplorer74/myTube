// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Converters.SecondsToTimeSpanStringConverter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
  public class SecondsToTimeSpanStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) value);
      return (object) string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => (object) TimeSpan.Parse((string) value);
  }
}
