// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.CursorDisplayHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class CursorDisplayHandler
  {
    private FrameworkElement _control;
    private bool _isHovering;
    private static CoreCursor _defaultCursor;

    private static CoreCursor DefaultCursor => CursorDisplayHandler._defaultCursor ?? (CursorDisplayHandler._defaultCursor = Window.Current.CoreWindow.PointerCursor);

    public void Attach(FrameworkElement frameworkElement)
    {
      this._control = frameworkElement;
      FrameworkElement control1 = this._control;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) control1).add_PointerEntered), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) control1).remove_PointerEntered), new PointerEventHandler(this.OnPointerEntered));
      FrameworkElement control2 = this._control;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) control2).add_PointerExited), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) control2).remove_PointerExited), new PointerEventHandler(this.OnPointerExited));
      FrameworkElement control3 = this._control;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(control3.add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(control3.remove_Unloaded), new RoutedEventHandler(this.OnControlUnloaded));
    }

    private void OnControlUnloaded(object sender, RoutedEventArgs e)
    {
    }

    public void Detach()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._control).remove_PointerEntered), new PointerEventHandler(this.OnPointerEntered));
      WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._control).remove_PointerExited), new PointerEventHandler(this.OnPointerExited));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._control.remove_Unloaded), new RoutedEventHandler(this.OnControlUnloaded));
      if (!this._isHovering)
        return;
      Window.Current.CoreWindow.put_PointerCursor(CursorDisplayHandler.DefaultCursor);
    }

    private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
    {
      if (e.Pointer.PointerDeviceType != 2)
        return;
      this._isHovering = true;
      this.UpdateCursor();
    }

    private void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
      if (e.Pointer.PointerDeviceType != 2)
        return;
      this._isHovering = false;
      Window.Current.CoreWindow.put_PointerCursor(CursorDisplayHandler.DefaultCursor);
    }

    internal void UpdateCursor()
    {
      if (CursorDisplayHandler._defaultCursor == null)
        CursorDisplayHandler._defaultCursor = Window.Current.CoreWindow.PointerCursor;
      CoreCursor cursor = FrameworkElementExtensions.GetCursor((DependencyObject) this._control);
      if (!this._isHovering)
        return;
      if (cursor != null)
        Window.Current.CoreWindow.put_PointerCursor(cursor);
      else
        Window.Current.CoreWindow.put_PointerCursor(CursorDisplayHandler.DefaultCursor);
    }
  }
}
