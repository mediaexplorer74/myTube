// Decompiled with JetBrains decompiler
// Type: myTube.LeftRightControl
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

namespace myTube
{
  public sealed class LeftRightControl : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl left;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl right;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler<DirectionTappedEventArgs> LeftRightTapped;

    public LeftRightControl() => this.InitializeComponent();

    private void left_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.LeftRightTapped == null)
        return;
      this.LeftRightTapped((object) this, new DirectionTappedEventArgs()
      {
        Direction = ControlDirection.Left
      });
    }

    private void right_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.LeftRightTapped == null)
        return;
      this.LeftRightTapped((object) this, new DirectionTappedEventArgs()
      {
        Direction = ControlDirection.Right
      });
    }

    private void right_PointerMoved(object sender, PointerRoutedEventArgs e) => e.put_Handled(true);

    private void left_PointerMoved(object sender, PointerRoutedEventArgs e) => e.put_Handled(true);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///LeftRightControl.xaml"), (ComponentResourceLocation) 0);
      this.left = (ContentControl) ((FrameworkElement) this).FindName("left");
      this.right = (ContentControl) ((FrameworkElement) this).FindName("right");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.left_Tapped));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement2.add_PointerMoved), new Action<EventRegistrationToken>(uiElement2.remove_PointerMoved), new PointerEventHandler(this.left_PointerMoved));
          break;
        case 2:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.right_Tapped));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement4.add_PointerMoved), new Action<EventRegistrationToken>(uiElement4.remove_PointerMoved), new PointerEventHandler(this.right_PointerMoved));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
