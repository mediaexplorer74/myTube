// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AlternativePage
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  public class AlternativePage : UserControl
  {
    public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof (Frame), (Type) typeof (AlternativeFrame), (Type) typeof (AlternativePage), new PropertyMetadata((object) null, new PropertyChangedCallback(AlternativePage.OnFrameChanged)));
    public static readonly DependencyProperty ShouldWaitForImagesToLoadProperty = DependencyProperty.Register(nameof (ShouldWaitForImagesToLoad), (Type) typeof (bool?), (Type) typeof (AlternativePage), new PropertyMetadata((object) null));
    public static readonly DependencyProperty NavigationCacheModeProperty = DependencyProperty.Register(nameof (NavigationCacheMode), (Type) typeof (NavigationCacheMode), (Type) typeof (AlternativePage), new PropertyMetadata((object) NavigationCacheMode.Disabled));
    public static readonly DependencyProperty NavigationStateProperty = DependencyProperty.Register(nameof (NavigationState), (Type) typeof (NavigationState), (Type) typeof (AlternativePage), new PropertyMetadata((object) NavigationState.Initializing));
    public static readonly DependencyProperty PageTransitionProperty = DependencyProperty.Register(nameof (PageTransition), (Type) typeof (PageTransition), (Type) typeof (AlternativePage), new PropertyMetadata((object) null));

    public AlternativeFrame Frame
    {
      get => (AlternativeFrame) ((DependencyObject) this).GetValue(AlternativePage.FrameProperty);
      internal set => ((DependencyObject) this).SetValue(AlternativePage.FrameProperty, (object) value);
    }

    private static void OnFrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AlternativePage alternativePage = (AlternativePage) d;
      AlternativeFrame oldValue = (AlternativeFrame) e.OldValue;
      AlternativeFrame frame = alternativePage.Frame;
      alternativePage.OnFrameChanged(oldValue, frame);
    }

    protected virtual void OnFrameChanged(AlternativeFrame oldFrame, AlternativeFrame newFrame)
    {
    }

    public bool? ShouldWaitForImagesToLoad
    {
      get => (bool?) ((DependencyObject) this).GetValue(AlternativePage.ShouldWaitForImagesToLoadProperty);
      set => ((DependencyObject) this).SetValue(AlternativePage.ShouldWaitForImagesToLoadProperty, (object) value);
    }

    public NavigationCacheMode NavigationCacheMode
    {
      get => (NavigationCacheMode) ((DependencyObject) this).GetValue(AlternativePage.NavigationCacheModeProperty);
      set => ((DependencyObject) this).SetValue(AlternativePage.NavigationCacheModeProperty, (object) value);
    }

    public NavigationState NavigationState
    {
      get => (NavigationState) ((DependencyObject) this).GetValue(AlternativePage.NavigationStateProperty);
      private set => ((DependencyObject) this).SetValue(AlternativePage.NavigationStateProperty, (object) value);
    }

    public PageTransition PageTransition
    {
      get => (PageTransition) ((DependencyObject) this).GetValue(AlternativePage.PageTransitionProperty);
      set => ((DependencyObject) this).SetValue(AlternativePage.PageTransitionProperty, (object) value);
    }

    protected virtual async Task OnNavigatedFrom(AlternativeNavigationEventArgs e)
    {
    }

    protected virtual async Task OnNavigatedTo(AlternativeNavigationEventArgs e)
    {
    }

    protected virtual async Task OnNavigatingFrom(AlternativeNavigatingCancelEventArgs e) => this.NavigationState = NavigationState.NavigatingFrom;

    protected virtual async Task OnNavigatingTo(AlternativeNavigationEventArgs e) => this.NavigationState = NavigationState.NavigatingTo;

    protected virtual async Task OnTransitioningTo()
    {
    }

    protected virtual async Task OnTransitionedTo()
    {
    }

    protected virtual async Task OnTransitioningFrom()
    {
    }

    protected virtual async Task OnTransitionedFrom()
    {
    }

    internal async Task OnTransitioningToInternal()
    {
      this.NavigationState = NavigationState.TransitioningTo;
      await this.OnTransitioningTo();
    }

    internal async Task OnTransitionedToInternal()
    {
      this.NavigationState = NavigationState.TransitionedTo;
      await this.OnTransitionedTo();
    }

    internal async Task OnTransitioningFromInternal()
    {
      this.NavigationState = NavigationState.TransitioningFrom;
      await this.OnTransitioningFrom();
    }

    internal async Task OnTransitionedFromInternal()
    {
      this.NavigationState = NavigationState.TransitionedFrom;
      await this.OnTransitionedFrom();
    }

    internal async Task OnNavigatingFromInternal(AlternativeNavigatingCancelEventArgs e)
    {
      this.NavigationState = NavigationState.NavigatingFrom;
      await this.OnNavigatingFrom(e);
    }

    internal async Task OnNavigatingToInternal(AlternativeNavigationEventArgs e)
    {
      this.NavigationState = NavigationState.NavigatingTo;
      await this.OnNavigatingTo(e);
    }

    internal async Task OnNavigatedFromInternal(AlternativeNavigationEventArgs e)
    {
      this.NavigationState = NavigationState.NavigatedFrom;
      await this.OnNavigatedFrom(e);
    }

    internal async Task OnNavigatedToInternal(AlternativeNavigationEventArgs e)
    {
      this.NavigationState = NavigationState.NavigatedTo;
      await this.OnNavigatedTo(e);
    }

    protected virtual async Task Preload(object parameter)
    {
    }

    protected virtual async Task UnloadPreloaded()
    {
    }

    internal async Task PreloadInternal(object parameter)
    {
      this.NavigationState = NavigationState.Preloading;
      await this.Preload(parameter);
      this.NavigationState = NavigationState.Preloaded;
    }

    internal async Task UnloadPreloadedInternal()
    {
      this.NavigationState = NavigationState.UnloadingPreloaded;
      await this.UnloadPreloaded();
      this.NavigationState = NavigationState.UnloadedPreloaded;
    }
  }
}
