// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.FrameworkElementExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class FrameworkElementExtensions
  {
    public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached("ClipToBounds", (Type) typeof (bool), (Type) typeof (FrameworkElementExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(FrameworkElementExtensions.OnClipToBoundsChanged)));
    public static readonly DependencyProperty ClipToBoundsHandlerProperty = DependencyProperty.RegisterAttached("ClipToBoundsHandler", (Type) typeof (ClipToBoundsHandler), (Type) typeof (FrameworkElementExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(FrameworkElementExtensions.OnClipToBoundsHandlerChanged)));
    public static readonly DependencyProperty CursorProperty = DependencyProperty.RegisterAttached("Cursor", (Type) typeof (CoreCursor), (Type) typeof (FrameworkElementExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(FrameworkElementExtensions.OnCursorChanged)));
    public static readonly DependencyProperty SystemCursorProperty = DependencyProperty.RegisterAttached("SystemCursor", (Type) typeof (CoreCursorType), (Type) typeof (FrameworkElementExtensions), new PropertyMetadata((object) (CoreCursorType) 0, new PropertyChangedCallback(FrameworkElementExtensions.OnSystemCursorChanged)));
    public static readonly DependencyProperty CursorDisplayHandlerProperty = DependencyProperty.RegisterAttached("CursorDisplayHandler", (Type) typeof (CursorDisplayHandler), (Type) typeof (FrameworkElementExtensions), new PropertyMetadata((object) null));

    public static bool GetClipToBounds(DependencyObject d) => (bool) d.GetValue(FrameworkElementExtensions.ClipToBoundsProperty);

    public static void SetClipToBounds(DependencyObject d, bool value) => d.SetValue(FrameworkElementExtensions.ClipToBoundsProperty, (object) value);

    private static void OnClipToBoundsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      int num = (bool) e.OldValue ? 1 : 0;
      if ((bool) d.GetValue(FrameworkElementExtensions.ClipToBoundsProperty))
        FrameworkElementExtensions.SetClipToBoundsHandler(d, new ClipToBoundsHandler());
      else
        FrameworkElementExtensions.SetClipToBoundsHandler(d, (ClipToBoundsHandler) null);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ClipToBoundsHandler GetClipToBoundsHandler(DependencyObject d) => (ClipToBoundsHandler) d.GetValue(FrameworkElementExtensions.ClipToBoundsHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetClipToBoundsHandler(DependencyObject d, ClipToBoundsHandler value) => d.SetValue(FrameworkElementExtensions.ClipToBoundsHandlerProperty, (object) value);

    private static void OnClipToBoundsHandlerChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ClipToBoundsHandler oldValue = (ClipToBoundsHandler) e.OldValue;
      ClipToBoundsHandler clipToBoundsHandler = (ClipToBoundsHandler) d.GetValue(FrameworkElementExtensions.ClipToBoundsHandlerProperty);
      oldValue?.Detach();
      clipToBoundsHandler?.Attach((FrameworkElement) d);
    }

    public static CoreCursor GetCursor(DependencyObject d) => (CoreCursor) d.GetValue(FrameworkElementExtensions.CursorProperty);

    public static void SetCursor(DependencyObject d, CoreCursor value) => d.SetValue(FrameworkElementExtensions.CursorProperty, (object) value);

    private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CoreCursor oldValue = (CoreCursor) e.OldValue;
      CoreCursor coreCursor = (CoreCursor) d.GetValue(FrameworkElementExtensions.CursorProperty);
      if (oldValue == null)
      {
        CursorDisplayHandler cursorDisplayHandler = new CursorDisplayHandler();
        cursorDisplayHandler.Attach((FrameworkElement) d);
        FrameworkElementExtensions.SetCursorDisplayHandler(d, cursorDisplayHandler);
      }
      else
      {
        CursorDisplayHandler cursorDisplayHandler = FrameworkElementExtensions.GetCursorDisplayHandler(d);
        if (coreCursor == null)
        {
          cursorDisplayHandler.Detach();
          FrameworkElementExtensions.SetCursorDisplayHandler(d, (CursorDisplayHandler) null);
        }
        else
          cursorDisplayHandler.UpdateCursor();
      }
    }

    public static CoreCursorType GetSystemCursor(DependencyObject d) => (CoreCursorType) d.GetValue(FrameworkElementExtensions.SystemCursorProperty);

    public static void SetSystemCursor(DependencyObject d, CoreCursorType value) => d.SetValue(FrameworkElementExtensions.SystemCursorProperty, (object) value);

    private static void OnSystemCursorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CoreCursorType oldValue = (CoreCursorType) e.OldValue;
      CoreCursorType coreCursorType = (CoreCursorType) d.GetValue(FrameworkElementExtensions.SystemCursorProperty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CursorDisplayHandler GetCursorDisplayHandler(DependencyObject d) => (CursorDisplayHandler) d.GetValue(FrameworkElementExtensions.CursorDisplayHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetCursorDisplayHandler(DependencyObject d, CursorDisplayHandler value) => d.SetValue(FrameworkElementExtensions.CursorDisplayHandlerProperty, (object) value);

    public static bool HasNonDefaultValue(this DependencyObject @this, DependencyProperty dp) => @this.ReadLocalValue(dp) != DependencyProperty.UnsetValue;
  }
}
