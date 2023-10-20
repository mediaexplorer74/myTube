// Decompiled with JetBrains decompiler
// Type: myTube.Overlays.SortingControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Overlays
{
  public sealed class SortingControl : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl up;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl down;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler<DirectionTappedEventArgs> SortingTapped;

    public SortingControl() => this.InitializeComponent();

    private void down_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.SortingTapped == null)
        return;
      e.put_Handled(true);
      this.SortingTapped((object) this, new DirectionTappedEventArgs()
      {
        Direction = ControlDirection.Down
      });
    }

    private void up_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.SortingTapped == null)
        return;
      e.put_Handled(true);
      this.SortingTapped((object) this, new DirectionTappedEventArgs()
      {
        Direction = ControlDirection.Up
      });
    }

    private void up_PointerPressed(object sender, PointerRoutedEventArgs e) => e.put_Handled(true);

    private void down_PointerPressed(object sender, PointerRoutedEventArgs e) => e.put_Handled(true);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Overlays/SortingControl.xaml"), (ComponentResourceLocation) 0);
      this.up = (ContentControl) ((FrameworkElement) this).FindName("up");
      this.down = (ContentControl) ((FrameworkElement) this).FindName("down");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.up_Tapped));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement2.add_PointerPressed), new Action<EventRegistrationToken>(uiElement2.remove_PointerPressed), new PointerEventHandler(this.up_PointerPressed));
          break;
        case 2:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.down_Tapped));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement4.add_PointerPressed), new Action<EventRegistrationToken>(uiElement4.remove_PointerPressed), new PointerEventHandler(this.down_PointerPressed));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
