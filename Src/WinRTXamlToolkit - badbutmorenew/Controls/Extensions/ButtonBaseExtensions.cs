// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ButtonBaseExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ButtonBaseExtensions
  {
    public static readonly DependencyProperty ButtonStateEventBehaviorProperty = DependencyProperty.RegisterAttached("ButtonStateEventBehavior", (Type) typeof (ButtonStateEventBehavior), (Type) typeof (ButtonBaseExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ButtonBaseExtensions.OnButtonStateEventBehaviorChanged)));

    public static ButtonStateEventBehavior GetButtonStateEventBehavior(DependencyObject d) => (ButtonStateEventBehavior) d.GetValue(ButtonBaseExtensions.ButtonStateEventBehaviorProperty);

    public static void SetButtonStateEventBehavior(
      DependencyObject d,
      ButtonStateEventBehavior value)
    {
      d.SetValue(ButtonBaseExtensions.ButtonStateEventBehaviorProperty, (object) value);
    }

    private static void OnButtonStateEventBehaviorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ButtonStateEventBehavior oldValue = (ButtonStateEventBehavior) e.OldValue;
      ButtonStateEventBehavior stateEventBehavior = (ButtonStateEventBehavior) d.GetValue(ButtonBaseExtensions.ButtonStateEventBehaviorProperty);
      oldValue?.Detach();
      stateEventBehavior?.Attach((ButtonBase) d);
    }
  }
}
