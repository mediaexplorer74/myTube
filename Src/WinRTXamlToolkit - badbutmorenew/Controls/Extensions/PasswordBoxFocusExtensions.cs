// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.PasswordBoxFocusExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class PasswordBoxFocusExtensions
  {
    public static readonly DependencyProperty AutoTabOnMaxLengthProperty = DependencyProperty.RegisterAttached("AutoTabOnMaxLength", (Type) typeof (bool), (Type) typeof (PasswordBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(PasswordBoxFocusExtensions.OnAutoTabOnMaxLengthChanged)));
    public static readonly DependencyProperty PasswordAutoTabOnMaxLengthHandlerProperty = DependencyProperty.RegisterAttached("PasswordAutoTabOnMaxLengthHandler", (Type) typeof (PasswordAutoTabOnMaxLengthHandler), (Type) typeof (PasswordBoxFocusExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AutoSelectOnFocusProperty = DependencyProperty.RegisterAttached("AutoSelectOnFocus", (Type) typeof (bool), (Type) typeof (PasswordBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(PasswordBoxFocusExtensions.OnAutoSelectOnFocusChanged)));
    public static readonly DependencyProperty PasswordAutoSelectOnFocusHandlerProperty = DependencyProperty.RegisterAttached("PasswordAutoSelectOnFocusHandler", (Type) typeof (PasswordAutoSelectOnFocusHandler), (Type) typeof (PasswordBoxFocusExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DisableSearchPaneOnFocusProperty = DependencyProperty.RegisterAttached("DisableSearchPaneOnFocus", (Type) typeof (bool), (Type) typeof (PasswordBoxFocusExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(PasswordBoxFocusExtensions.OnDisableSearchPaneOnFocusChanged)));

    public static bool GetAutoTabOnMaxLength(DependencyObject d) => (bool) d.GetValue(PasswordBoxFocusExtensions.AutoTabOnMaxLengthProperty);

    public static void SetAutoTabOnMaxLength(DependencyObject d, bool value) => d.SetValue(PasswordBoxFocusExtensions.AutoTabOnMaxLengthProperty, (object) value);

    private static void OnAutoTabOnMaxLengthChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      bool flag = (bool) d.GetValue(PasswordBoxFocusExtensions.AutoTabOnMaxLengthProperty);
      PasswordAutoTabOnMaxLengthHandler maxLengthHandler1 = PasswordBoxFocusExtensions.GetPasswordAutoTabOnMaxLengthHandler(d);
      if (maxLengthHandler1 != null)
      {
        maxLengthHandler1.Detach();
        PasswordBoxFocusExtensions.SetPasswordAutoTabOnMaxLengthHandler(d, (PasswordAutoTabOnMaxLengthHandler) null);
      }
      if (!flag)
        return;
      PasswordAutoTabOnMaxLengthHandler maxLengthHandler2 = new PasswordAutoTabOnMaxLengthHandler((PasswordBox) d);
      PasswordBoxFocusExtensions.SetPasswordAutoTabOnMaxLengthHandler(d, maxLengthHandler2);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static PasswordAutoTabOnMaxLengthHandler GetPasswordAutoTabOnMaxLengthHandler(
      DependencyObject d)
    {
      return (PasswordAutoTabOnMaxLengthHandler) d.GetValue(PasswordBoxFocusExtensions.PasswordAutoTabOnMaxLengthHandlerProperty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetPasswordAutoTabOnMaxLengthHandler(
      DependencyObject d,
      PasswordAutoTabOnMaxLengthHandler value)
    {
      d.SetValue(PasswordBoxFocusExtensions.PasswordAutoTabOnMaxLengthHandlerProperty, (object) value);
    }

    public static bool GetAutoSelectOnFocus(DependencyObject d) => (bool) d.GetValue(PasswordBoxFocusExtensions.AutoSelectOnFocusProperty);

    public static void SetAutoSelectOnFocus(DependencyObject d, bool value) => d.SetValue(PasswordBoxFocusExtensions.AutoSelectOnFocusProperty, (object) value);

    private static void OnAutoSelectOnFocusChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      bool flag = (bool) d.GetValue(PasswordBoxFocusExtensions.AutoSelectOnFocusProperty);
      PasswordAutoSelectOnFocusHandler selectOnFocusHandler1 = PasswordBoxFocusExtensions.GetPasswordAutoSelectOnFocusHandler(d);
      if (selectOnFocusHandler1 != null)
      {
        selectOnFocusHandler1.Detach();
        PasswordBoxFocusExtensions.SetPasswordAutoSelectOnFocusHandler(d, (PasswordAutoSelectOnFocusHandler) null);
      }
      if (!flag)
        return;
      PasswordAutoSelectOnFocusHandler selectOnFocusHandler2 = new PasswordAutoSelectOnFocusHandler((PasswordBox) d);
      PasswordBoxFocusExtensions.SetPasswordAutoSelectOnFocusHandler(d, selectOnFocusHandler2);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static PasswordAutoSelectOnFocusHandler GetPasswordAutoSelectOnFocusHandler(
      DependencyObject d)
    {
      return (PasswordAutoSelectOnFocusHandler) d.GetValue(PasswordBoxFocusExtensions.PasswordAutoSelectOnFocusHandlerProperty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetPasswordAutoSelectOnFocusHandler(
      DependencyObject d,
      PasswordAutoSelectOnFocusHandler value)
    {
      d.SetValue(PasswordBoxFocusExtensions.PasswordAutoSelectOnFocusHandlerProperty, (object) value);
    }

    public static bool GetDisableSearchPaneOnFocus(DependencyObject d) => (bool) d.GetValue(PasswordBoxFocusExtensions.DisableSearchPaneOnFocusProperty);

    public static void SetDisableSearchPaneOnFocus(DependencyObject d, bool value) => d.SetValue(PasswordBoxFocusExtensions.DisableSearchPaneOnFocusProperty, (object) value);

    private static void OnDisableSearchPaneOnFocusChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }
  }
}
