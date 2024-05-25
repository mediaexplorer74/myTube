// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ScrollViewerExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ScrollViewerExtensions
  {
    private static readonly TimeSpan DefaultAnimatedScrollDuration = TimeSpan.FromSeconds(1.5);
    private static readonly EasingFunctionBase DefaultEasingFunction;
    public static readonly DependencyProperty AnimatedScrollHandlerProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ScrollViewerAnimatedScrollHandler GetAnimatedScrollHandler(DependencyObject d) => (ScrollViewerAnimatedScrollHandler) d.GetValue(ScrollViewerExtensions.AnimatedScrollHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetAnimatedScrollHandler(
      DependencyObject d,
      ScrollViewerAnimatedScrollHandler value)
    {
      d.SetValue(ScrollViewerExtensions.AnimatedScrollHandlerProperty, (object) value);
    }

    private static void OnAnimatedScrollHandlerChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ScrollViewerAnimatedScrollHandler oldValue = (ScrollViewerAnimatedScrollHandler) e.OldValue;
      ScrollViewerAnimatedScrollHandler animatedScrollHandler = (ScrollViewerAnimatedScrollHandler) d.GetValue(ScrollViewerExtensions.AnimatedScrollHandlerProperty);
      ScrollViewer scrollViewer = d as ScrollViewer;
      oldValue?.Detach();
      animatedScrollHandler?.Attach(scrollViewer);
    }

    public static async Task ScrollToHorizontalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset)
    {
      await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset, ScrollViewerExtensions.DefaultAnimatedScrollDuration);
    }

    public static async Task ScrollToHorizontalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      double durationInSeconds)
    {
      await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset, TimeSpan.FromSeconds(durationInSeconds), ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ScrollToHorizontalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      double durationInSeconds,
      EasingFunctionBase easingFunction)
    {
      await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset, TimeSpan.FromSeconds(durationInSeconds), easingFunction);
    }

    public static async Task ScrollToHorizontalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      TimeSpan duration)
    {
      await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset, duration, ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ScrollToHorizontalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      ScrollViewerAnimatedScrollHandler handler = ScrollViewerExtensions.GetAnimatedScrollHandler((DependencyObject) scrollViewer);
      if (handler == null)
      {
        handler = new ScrollViewerAnimatedScrollHandler();
        ScrollViewerExtensions.SetAnimatedScrollHandler((DependencyObject) scrollViewer, handler);
      }
      await handler.ScrollToHorizontalOffsetWithAnimation(offset, duration, easingFunction);
    }

    public static async Task ScrollToVerticalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset)
    {
      await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset, ScrollViewerExtensions.DefaultAnimatedScrollDuration);
    }

    public static async Task ScrollToVerticalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      double durationInSeconds)
    {
      await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset, TimeSpan.FromSeconds(durationInSeconds), ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ScrollToVerticalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      double durationInSeconds,
      EasingFunctionBase easingFunction)
    {
      await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset, TimeSpan.FromSeconds(durationInSeconds), easingFunction);
    }

    public static async Task ScrollToVerticalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      TimeSpan duration)
    {
      await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset, duration, ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ScrollToVerticalOffsetWithAnimation(
      this ScrollViewer scrollViewer,
      double offset,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      ScrollViewerAnimatedScrollHandler handler = ScrollViewerExtensions.GetAnimatedScrollHandler((DependencyObject) scrollViewer);
      if (handler == null)
      {
        handler = new ScrollViewerAnimatedScrollHandler();
        ScrollViewerExtensions.SetAnimatedScrollHandler((DependencyObject) scrollViewer, handler);
      }
      await handler.ScrollToVerticalOffsetWithAnimation(offset, duration, easingFunction);
    }

    public static async Task ZoomToFactorWithAnimation(
      this ScrollViewer scrollViewer,
      double factor)
    {
      await scrollViewer.ZoomToFactorWithAnimation(factor, ScrollViewerExtensions.DefaultAnimatedScrollDuration);
    }

    public static async Task ZoomToFactorWithAnimation(
      this ScrollViewer scrollViewer,
      double factor,
      double durationInSeconds)
    {
      await scrollViewer.ZoomToFactorWithAnimation(factor, TimeSpan.FromSeconds(durationInSeconds), ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ZoomToFactorWithAnimation(
      this ScrollViewer scrollViewer,
      double factor,
      double durationInSeconds,
      EasingFunctionBase easingFunction)
    {
      await scrollViewer.ZoomToFactorWithAnimation(factor, TimeSpan.FromSeconds(durationInSeconds), easingFunction);
    }

    public static async Task ZoomToFactorWithAnimation(
      this ScrollViewer scrollViewer,
      double factor,
      TimeSpan duration)
    {
      await scrollViewer.ZoomToFactorWithAnimation(factor, duration, ScrollViewerExtensions.DefaultEasingFunction);
    }

    public static async Task ZoomToFactorWithAnimation(
      this ScrollViewer scrollViewer,
      double factor,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      ScrollViewerAnimatedScrollHandler handler = ScrollViewerExtensions.GetAnimatedScrollHandler((DependencyObject) scrollViewer);
      if (handler == null)
      {
        handler = new ScrollViewerAnimatedScrollHandler();
        ScrollViewerExtensions.SetAnimatedScrollHandler((DependencyObject) scrollViewer, handler);
      }
      await handler.ZoomToFactorWithAnimation(factor, duration, easingFunction);
    }

    static ScrollViewerExtensions()
    {
      CubicEase cubicEase = new CubicEase();
      ((EasingFunctionBase) cubicEase).put_EasingMode((EasingMode) 0);
      ScrollViewerExtensions.DefaultEasingFunction = (EasingFunctionBase) cubicEase;
      ScrollViewerExtensions.AnimatedScrollHandlerProperty = DependencyProperty.RegisterAttached("AnimatedScrollHandler", (Type) typeof (ScrollViewerAnimatedScrollHandler), (Type) typeof (ScrollViewerExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ScrollViewerExtensions.OnAnimatedScrollHandlerChanged)));
    }
  }
}
