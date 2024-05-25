// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.VisualStateExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class VisualStateExtensions
  {
    public static readonly DependencyProperty StateProperty = DependencyProperty.RegisterAttached("State", (Type) typeof (string), (Type) typeof (VisualStateExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(VisualStateExtensions.OnStateChanged)));

    public static string GetState(DependencyObject d) => (string) d.GetValue(VisualStateExtensions.StateProperty);

    public static void SetState(DependencyObject d, string value) => d.SetValue(VisualStateExtensions.StateProperty, (object) value);

    private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      string newValue = (string) e.NewValue;
      VisualStateManager.GoToState((Control) d, newValue, true);
    }
  }
}
