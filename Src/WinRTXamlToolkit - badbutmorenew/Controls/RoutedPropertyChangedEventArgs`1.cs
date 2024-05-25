// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.RoutedPropertyChangedEventArgs`1
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
  public class RoutedPropertyChangedEventArgs<T> : RoutedEventArgs
  {
    public RoutedPropertyChangedEventArgs(T oldValue, T newValue)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public T NewValue { get; private set; }

    public T OldValue { get; private set; }
  }
}
