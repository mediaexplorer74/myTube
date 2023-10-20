// Decompiled with JetBrains decompiler
// Type: myTube.VisualStateHelpers.PointerStateHelper
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace myTube.VisualStateHelpers
{
  public class PointerStateHelper
  {
    public static DependencyProperty PointerStateHelperProperty = DependencyProperty.RegisterAttached(nameof (PointerStateHelper), typeof (bool), typeof (UIElement), new PropertyMetadata((object) false, new PropertyChangedCallback(PointerStateHelper.OnPointerStateHelperPropertyChanged)));
    public static DependencyProperty PointerStateBackgroundProperty = DependencyProperty.RegisterAttached("PointerStateBackground", typeof (Brush), typeof (UIElement), new PropertyMetadata((object) null));

    public static Brush GetPointerStateBackground(DependencyObject obj) => (Brush) obj.GetValue(PointerStateHelper.PointerStateBackgroundProperty);

    public static void SetPointerStateBackground(DependencyObject obj, Brush value) => obj.SetValue(PointerStateHelper.PointerStateBackgroundProperty, (object) value);

    private static void OnPointerStateHelperPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      UIElement uiElement1 = d as UIElement;
      bool newValue = (bool) e.NewValue;
      if (uiElement1 == null)
        return;
      if (newValue)
      {
        UIElement uiElement2 = uiElement1;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement2.add_PointerEntered), new Action<EventRegistrationToken>(uiElement2.remove_PointerEntered), new PointerEventHandler(PointerStateHelper.PointerEntered));
        UIElement uiElement3 = uiElement1;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement3.add_PointerPressed), new Action<EventRegistrationToken>(uiElement3.remove_PointerPressed), new PointerEventHandler(PointerStateHelper.PointerPressed));
        UIElement uiElement4 = uiElement1;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement4.add_PointerExited), new Action<EventRegistrationToken>(uiElement4.remove_PointerExited), new PointerEventHandler(PointerStateHelper.PointerExited));
        UIElement uiElement5 = uiElement1;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement5.add_PointerCaptureLost), new Action<EventRegistrationToken>(uiElement5.remove_PointerCaptureLost), new PointerEventHandler(PointerStateHelper.PointerCaptureLost));
      }
      else
      {
        WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>(new Action<EventRegistrationToken>(uiElement1.remove_PointerEntered), new PointerEventHandler(PointerStateHelper.PointerEntered));
        WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>(new Action<EventRegistrationToken>(uiElement1.remove_PointerPressed), new PointerEventHandler(PointerStateHelper.PointerPressed));
        WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>(new Action<EventRegistrationToken>(uiElement1.remove_PointerExited), new PointerEventHandler(PointerStateHelper.PointerExited));
        WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>(new Action<EventRegistrationToken>(uiElement1.remove_PointerCaptureLost), new PointerEventHandler(PointerStateHelper.PointerCaptureLost));
      }
    }

    private static void PointerExited(object sender, PointerRoutedEventArgs e)
    {
      if (!(sender is Control control))
        return;
      PointerStateHelper.tryState(control, "PointerUp");
      ((UIElement) control).ReleasePointerCaptures();
    }

    private static void PointerCaptureLost(object sender, PointerRoutedEventArgs e)
    {
      if (!(sender is Control control))
        return;
      PointerStateHelper.tryState(control, "PointerUp");
    }

    private static void PointerPressed(object sender, PointerRoutedEventArgs e)
    {
      if (!(sender is Control control))
        return;
      ((UIElement) control).CapturePointer(e.Pointer);
      PointerStateHelper.tryState(control, "PointerDown");
    }

    private static void PointerEntered(object sender, PointerRoutedEventArgs e)
    {
      if (!(sender is Control control) || e.Pointer.IsInContact)
        return;
      ((UIElement) control).CapturePointer(e.Pointer);
      PointerStateHelper.tryState(control, nameof (PointerEntered));
    }

    private static bool tryState(Control obj, string state)
    {
      try
      {
        VisualStateManager.GetVisualStateGroups((FrameworkElement) obj);
        return VisualStateManager.GoToState(obj, state, true);
      }
      catch
      {
        return false;
      }
    }

    public static void SetPointerStateHelper(DependencyObject obj, bool value) => obj.SetValue(PointerStateHelper.PointerStateHelperProperty, (object) value);

    public static bool GetPointerStateHelper(DependencyObject obj) => (bool) obj.GetValue(PointerStateHelper.PointerStateHelperProperty);
  }
}
