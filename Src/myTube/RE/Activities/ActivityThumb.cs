// Decompiled with JetBrains decompiler
// Type: myTube.Activities.ActivityThumb
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Activities
{
  public sealed class ActivityThumb : UserControl, IComponentConnector
  {
    public static DependencyProperty BoldTextProperty = DependencyProperty.Register(nameof (BoldText), typeof (string), typeof (ActivityThumb), new PropertyMetadata((object) ""));
    public static DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ActivityThumb), new PropertyMetadata((object) "activity"));
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition leftColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition rightColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup sizeStates;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup textStates;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup pointerStates;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Normal;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerDown;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerUp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState DefaultText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NarrowText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Default;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Wide;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid mainCard;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid rightGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock bigTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock description;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock activityTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Control16by9 imageContainer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock mainTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public string BoldText
    {
      get => (string) ((DependencyObject) this).GetValue(ActivityThumb.BoldTextProperty);
      set => ((DependencyObject) this).SetValue(ActivityThumb.BoldTextProperty, (object) value);
    }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(ActivityThumb.TextProperty);
      set => ((DependencyObject) this).SetValue(ActivityThumb.TextProperty, (object) value);
    }

    public ActivityThumb()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.ActivityThumb_SizeChanged));
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.ActivityThumb_Tapped));
    }

    private void ActivityThumb_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      App.Instance.RootFrame.Navigate(typeof (VideoPage), ((FrameworkElement) this).DataContext);
    }

    private void ActivityThumb_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Width > 600.0)
        VisualStateManager.GoToState((Control) this, "Wide", true);
      else
        VisualStateManager.GoToState((Control) this, "Default", true);
    }

    private void mainCard_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Width < 400.0)
        VisualStateManager.GoToState((Control) this, "NarrowText", true);
      else
        VisualStateManager.GoToState((Control) this, "DefaultText", true);
    }

    private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry dataContext) || dataContext.ActivityType != YouTubeActivity.Upload && dataContext.ActivityType != YouTubeActivity.Like)
        return;
      e.put_Handled(true);
      App.Instance.RootFrame.Navigate(typeof (ChannelPage), (object) dataContext.Author);
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

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Activities/ActivityThumb.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.leftColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("leftColumn");
      this.rightColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("rightColumn");
      this.sizeStates = (VisualStateGroup) ((FrameworkElement) this).FindName("sizeStates");
      this.textStates = (VisualStateGroup) ((FrameworkElement) this).FindName("textStates");
      this.pointerStates = (VisualStateGroup) ((FrameworkElement) this).FindName("pointerStates");
      this.Normal = (VisualState) ((FrameworkElement) this).FindName("Normal");
      this.PointerDown = (VisualState) ((FrameworkElement) this).FindName("PointerDown");
      this.PointerUp = (VisualState) ((FrameworkElement) this).FindName("PointerUp");
      this.DefaultText = (VisualState) ((FrameworkElement) this).FindName("DefaultText");
      this.NarrowText = (VisualState) ((FrameworkElement) this).FindName("NarrowText");
      this.Default = (VisualState) ((FrameworkElement) this).FindName("Default");
      this.Wide = (VisualState) ((FrameworkElement) this).FindName("Wide");
      this.mainCard = (Grid) ((FrameworkElement) this).FindName("mainCard");
      this.rightGrid = (Grid) ((FrameworkElement) this).FindName("rightGrid");
      this.bigTitle = (TextBlock) ((FrameworkElement) this).FindName("bigTitle");
      this.description = (TextBlock) ((FrameworkElement) this).FindName("description");
      this.activityTitle = (TextBlock) ((FrameworkElement) this).FindName("activityTitle");
      this.imageContainer = (Control16by9) ((FrameworkElement) this).FindName("imageContainer");
      this.mainTitle = (TextBlock) ((FrameworkElement) this).FindName("mainTitle");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement.add_SizeChanged), new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), new SizeChangedEventHandler(this.mainCard_SizeChanged));
          break;
        case 2:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.TextBlock_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
