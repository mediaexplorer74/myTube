// Decompiled with JetBrains decompiler
// Type: myTube.Overlays.SelectControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace myTube.Overlays
{
  public sealed class SelectControl : UserControl, IComponentConnector
  {
    public static DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (SelectControl), new PropertyMetadata((object) false, new PropertyChangedCallback(SelectControl.OnSelectedChanged)));
    private bool loaded;
    private ISelectableThumbnail thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform checkTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Selected;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Unselected;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle rect;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock check;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SelectControl selectControl = d as SelectControl;
      bool newValue = (bool) e.NewValue;
      if (!selectControl.loaded)
        return;
      VisualStateManager.GoToState((Control) selectControl, newValue ? "Selected" : "Unselected", true);
    }

    private bool IsSelected => (bool) ((DependencyObject) this).GetValue(SelectControl.IsSelectedProperty);

    public SelectControl()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.SelectControl_Tapped));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.SelectControl_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.SelectControl_Unloaded));
    }

    private void SelectControl_Unloaded(object sender, RoutedEventArgs e) => this.loaded = false;

    private void SelectControl_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.IsSelected)
        VisualStateManager.GoToState((Control) this, "Selected", true);
      else
        VisualStateManager.GoToState((Control) this, "Unselected", true);
      this.loaded = true;
    }

    private void SelectControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.thumb == null)
        return;
      this.thumb.Selected = !this.thumb.Selected;
      e.put_Handled(true);
    }

    private void Thumb_SelectChanged(object sender, bool e) => ((DependencyObject) this).SetValue(SelectControl.IsSelectedProperty, (object) e);

    public void ConnectToThumnbnail(ISelectableThumbnail thumb)
    {
      this.DisconnectThumbnail();
      this.thumb = thumb;
      ((DependencyObject) this).SetValue(SelectControl.IsSelectedProperty, (object) thumb.Selected);
      thumb.SelectChanged += new EventHandler<bool>(this.Thumb_SelectChanged);
    }

    public void DisconnectThumbnail()
    {
      if (this.thumb == null)
        return;
      this.thumb.SelectChanged -= new EventHandler<bool>(this.Thumb_SelectChanged);
      this.thumb = (ISelectableThumbnail) null;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Overlays/SelectControl.xaml"), (ComponentResourceLocation) 0);
      this.checkTrans = (CompositeTransform) ((FrameworkElement) this).FindName("checkTrans");
      this.Selected = (VisualState) ((FrameworkElement) this).FindName("Selected");
      this.Unselected = (VisualState) ((FrameworkElement) this).FindName("Unselected");
      this.rect = (Rectangle) ((FrameworkElement) this).FindName("rect");
      this.check = (TextBlock) ((FrameworkElement) this).FindName("check");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
