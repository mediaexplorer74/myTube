// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.TextBoxFocusExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class TextBoxFocusExtensions
  {
    public static readonly DependencyProperty AutoTabOnMaxLengthProperty = DependencyProperty.RegisterAttached("AutoTabOnMaxLength", (Type) typeof (bool), (Type) typeof (TextBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(TextBoxFocusExtensions.OnAutoTabOnMaxLengthChanged)));
    public static readonly DependencyProperty AutoTabOnMaxLengthHandlerProperty = DependencyProperty.RegisterAttached("AutoTabOnMaxLengthHandler", (Type) typeof (AutoTabOnMaxLengthHandler), (Type) typeof (TextBoxFocusExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AutoSelectOnFocusProperty = DependencyProperty.RegisterAttached("AutoSelectOnFocus", (Type) typeof (bool), (Type) typeof (TextBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(TextBoxFocusExtensions.OnAutoSelectOnFocusChanged)));
    public static readonly DependencyProperty AutoSelectOnFocusHandlerProperty = DependencyProperty.RegisterAttached("AutoSelectOnFocusHandler", (Type) typeof (AutoSelectOnFocusHandler), (Type) typeof (TextBoxFocusExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DisableSearchPaneOnFocusProperty = DependencyProperty.RegisterAttached("DisableSearchPaneOnFocus", (Type) typeof (bool), (Type) typeof (TextBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(TextBoxFocusExtensions.OnDisableSearchPaneOnFocusChanged)));

    public static bool GetAutoTabOnMaxLength(DependencyObject d) => (bool) d.GetValue(TextBoxFocusExtensions.AutoTabOnMaxLengthProperty);

    public static void SetAutoTabOnMaxLength(DependencyObject d, bool value) => d.SetValue(TextBoxFocusExtensions.AutoTabOnMaxLengthProperty, (object) value);

    private static void OnAutoTabOnMaxLengthChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      bool flag = (bool) d.GetValue(TextBoxFocusExtensions.AutoTabOnMaxLengthProperty);
      AutoTabOnMaxLengthHandler maxLengthHandler1 = TextBoxFocusExtensions.GetAutoTabOnMaxLengthHandler(d);
      if (maxLengthHandler1 != null)
      {
        maxLengthHandler1.Detach();
        TextBoxFocusExtensions.SetAutoTabOnMaxLengthHandler(d, (AutoTabOnMaxLengthHandler) null);
      }
      if (!flag)
        return;
      AutoTabOnMaxLengthHandler maxLengthHandler2 = new AutoTabOnMaxLengthHandler((TextBox) d);
      TextBoxFocusExtensions.SetAutoTabOnMaxLengthHandler(d, maxLengthHandler2);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static AutoTabOnMaxLengthHandler GetAutoTabOnMaxLengthHandler(DependencyObject d) => (AutoTabOnMaxLengthHandler) d.GetValue(TextBoxFocusExtensions.AutoTabOnMaxLengthHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetAutoTabOnMaxLengthHandler(
      DependencyObject d,
      AutoTabOnMaxLengthHandler value)
    {
      d.SetValue(TextBoxFocusExtensions.AutoTabOnMaxLengthHandlerProperty, (object) value);
    }

    public static bool GetAutoSelectOnFocus(DependencyObject d) => (bool) d.GetValue(TextBoxFocusExtensions.AutoSelectOnFocusProperty);

    public static void SetAutoSelectOnFocus(DependencyObject d, bool value) => d.SetValue(TextBoxFocusExtensions.AutoSelectOnFocusProperty, (object) value);

    private static void OnAutoSelectOnFocusChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      bool flag = (bool) d.GetValue(TextBoxFocusExtensions.AutoSelectOnFocusProperty);
      AutoSelectOnFocusHandler selectOnFocusHandler1 = TextBoxFocusExtensions.GetAutoSelectOnFocusHandler(d);
      if (selectOnFocusHandler1 != null)
      {
        selectOnFocusHandler1.Detach();
        TextBoxFocusExtensions.SetAutoSelectOnFocusHandler(d, (AutoSelectOnFocusHandler) null);
      }
      if (!flag)
        return;
      AutoSelectOnFocusHandler selectOnFocusHandler2 = new AutoSelectOnFocusHandler((TextBox) d);
      TextBoxFocusExtensions.SetAutoSelectOnFocusHandler(d, selectOnFocusHandler2);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static AutoSelectOnFocusHandler GetAutoSelectOnFocusHandler(DependencyObject d) => (AutoSelectOnFocusHandler) d.GetValue(TextBoxFocusExtensions.AutoSelectOnFocusHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetAutoSelectOnFocusHandler(
      DependencyObject d,
      AutoSelectOnFocusHandler value)
    {
      d.SetValue(TextBoxFocusExtensions.AutoSelectOnFocusHandlerProperty, (object) value);
    }

    public static bool GetDisableSearchPaneOnFocus(DependencyObject d) => (bool) d.GetValue(TextBoxFocusExtensions.DisableSearchPaneOnFocusProperty);

    public static void SetDisableSearchPaneOnFocus(DependencyObject d, bool value) => d.SetValue(TextBoxFocusExtensions.DisableSearchPaneOnFocusProperty, (object) value);

    private static void OnDisableSearchPaneOnFocusChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }
  }
}
