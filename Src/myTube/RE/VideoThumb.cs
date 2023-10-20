// Decompiled with JetBrains decompiler
// Type: myTube.VideoThumb
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using myTube.Overlays;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class VideoThumb : UserControl, ISortableThumbnail<YouTubeEntry>, IComponentConnector
  {
    public static DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (VideoThumb), new PropertyMetadata((object) "TITLE..."));
    private SortingControl sortingControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Border layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Normal;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerDown;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerUp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid mainGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock TitleText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock Time;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock Views;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public string Title
    {
      get => (string) ((DependencyObject) this).GetValue(VideoThumb.TitleProperty);
      set => ((DependencyObject) this).SetValue(VideoThumb.TitleProperty, (object) value);
    }

    public VideoThumb()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.VideoThumb_Tapped));
    }

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      ((UIElement) this).CapturePointer(e.Pointer);
      VisualStateManager.GoToState((Control) this, "PointerDown", true);
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e) => ((UIElement) this).ReleasePointerCapture(e.Pointer);

    protected virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "PointerUp", true);
      ((Control) this).OnPointerCaptureLost(e);
    }

    private void VideoThumb_Tapped(object sender, TappedRoutedEventArgs e) => VideoList.TappedThumb((FrameworkElement) this);

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      this.Content.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
      return finalSize;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(200.0, 200.0);
      if (availableSize.Width > availableSize.Height || double.IsInfinity(availableSize.Height))
      {
        size.Width = availableSize.Width;
        size.Height = availableSize.Width * 9.0 / 16.0;
      }
      else if (availableSize.Height > availableSize.Width || double.IsInfinity(availableSize.Width))
      {
        size.Height = availableSize.Height;
        size.Width = availableSize.Width * 16.0 / 9.0;
      }
      if (size.Height > availableSize.Height)
        size.Height = availableSize.Height;
      if (size.Width > availableSize.Width)
        size.Width = availableSize.Width;
      if (double.IsInfinity(size.Width) || double.IsInfinity(size.Height) || double.IsNaN(size.Width) || double.IsNaN(size.Height))
      {
        size.Width = 300.0;
        size.Height = 200.0;
      }
      this.Content.Measure(size);
      return size;
    }

    private void UserControl_RightTapped(object sender, RightTappedRoutedEventArgs e) => VideoList.RightTappedThumb((FrameworkElement) this, e);

    private void UserControl_Holding(object sender, HoldingRoutedEventArgs e) => VideoList.HoldingThumb((FrameworkElement) this, e);

    public void BeginSorting()
    {
      if (this.sortingControl == null)
      {
        this.sortingControl = new SortingControl();
        Grid.SetRowSpan((FrameworkElement) this.sortingControl, 4);
        Grid.SetColumnSpan((FrameworkElement) this.sortingControl, 4);
        this.sortingControl.SortingTapped += new EventHandler<DirectionTappedEventArgs>(this.sortingControl_SortingTapped);
      }
      if (((ICollection<UIElement>) ((Panel) this.mainGrid).Children).Contains((UIElement) this.sortingControl))
        return;
      ((ICollection<UIElement>) ((Panel) this.mainGrid).Children).Add((UIElement) this.sortingControl);
    }

    private void sortingControl_SortingTapped(object sender, DirectionTappedEventArgs e)
    {
      if (this.SortTapped == null)
        return;
      this.SortTapped((object) this, new SortTappedEventArgs<YouTubeEntry>()
      {
        Direction = e.Direction,
        Object = ((FrameworkElement) this).DataContext as YouTubeEntry
      });
    }

    public void EndSorting()
    {
      if (this.sortingControl == null || !((ICollection<UIElement>) ((Panel) this.mainGrid).Children).Contains((UIElement) this.sortingControl))
        return;
      ((ICollection<UIElement>) ((Panel) this.mainGrid).Children).Remove((UIElement) this.sortingControl);
    }

    public event EventHandler<SortTappedEventArgs<YouTubeEntry>> SortTapped;

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///VideoThumb.xaml"), (ComponentResourceLocation) 0);
      this.layoutRoot = (Border) ((FrameworkElement) this).FindName("layoutRoot");
      this.Normal = (VisualState) ((FrameworkElement) this).FindName("Normal");
      this.PointerDown = (VisualState) ((FrameworkElement) this).FindName("PointerDown");
      this.PointerUp = (VisualState) ((FrameworkElement) this).FindName("PointerUp");
      this.mainGrid = (Grid) ((FrameworkElement) this).FindName("mainGrid");
      this.TitleText = (TextBlock) ((FrameworkElement) this).FindName("TitleText");
      this.Time = (TextBlock) ((FrameworkElement) this).FindName("Time");
      this.Views = (TextBlock) ((FrameworkElement) this).FindName("Views");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement1 = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement1.add_RightTapped), new Action<EventRegistrationToken>(uiElement1.remove_RightTapped), new RightTappedEventHandler(this.UserControl_RightTapped));
        UIElement uiElement2 = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement2.add_Holding), new Action<EventRegistrationToken>(uiElement2.remove_Holding), new HoldingEventHandler(this.UserControl_Holding));
      }
      this._contentLoaded = true;
    }
  }
}
