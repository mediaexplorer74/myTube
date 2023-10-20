// Decompiled with JetBrains decompiler
// Type: myTube.IconTextButton
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
  public sealed class IconTextButton : UserControl, IComponentConnector
  {
    public static DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (IconTextButton), new PropertyMetadata((object) null));
    public static DependencyProperty SymbolProperty = DependencyProperty.Register(nameof (Symbol), typeof (Symbol), typeof (IconTextButton), new PropertyMetadata((object) (Symbol) 57609));
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Normal;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerUp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerDown;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(IconTextButton.TextProperty);
      set => ((DependencyObject) this).SetValue(IconTextButton.TextProperty, (object) value);
    }

    public Symbol Symbol
    {
      get => (Symbol) ((DependencyObject) this).GetValue(IconTextButton.SymbolProperty);
      set => ((DependencyObject) this).SetValue(IconTextButton.SymbolProperty, (object) value);
    }

    public IconTextButton() => this.InitializeComponent();

    protected virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
    {
      ((Control) this).OnPointerCaptureLost(e);
      VisualStateManager.GoToState((Control) this, "PointerUp", true);
    }

    private void userControl_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
      ((UIElement) this).CapturePointer(e.Pointer);
      VisualStateManager.GoToState((Control) this, "PointerDown", true);
    }

    private void layoutRoot_PointerMoved(object sender, PointerRoutedEventArgs e) => ((UIElement) this).ReleasePointerCapture(e.Pointer);

    private void layoutRoot_PointerReleased(object sender, PointerRoutedEventArgs e) => ((UIElement) this).ReleasePointerCapture(e.Pointer);

    private void layoutRoot_PointerExited(object sender, PointerRoutedEventArgs e) => ((UIElement) this).ReleasePointerCapture(e.Pointer);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///IconTextButton.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.Normal = (VisualState) ((FrameworkElement) this).FindName("Normal");
      this.PointerUp = (VisualState) ((FrameworkElement) this).FindName("PointerUp");
      this.PointerDown = (VisualState) ((FrameworkElement) this).FindName("PointerDown");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement1.add_PointerPressed), new Action<EventRegistrationToken>(uiElement1.remove_PointerPressed), new PointerEventHandler(this.userControl_PointerPressed));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement2.add_PointerMoved), new Action<EventRegistrationToken>(uiElement2.remove_PointerMoved), new PointerEventHandler(this.layoutRoot_PointerMoved));
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement3.add_PointerReleased), new Action<EventRegistrationToken>(uiElement3.remove_PointerReleased), new PointerEventHandler(this.layoutRoot_PointerReleased));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement4.add_PointerExited), new Action<EventRegistrationToken>(uiElement4.remove_PointerExited), new PointerEventHandler(this.layoutRoot_PointerExited));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
