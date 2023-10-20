// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ScrollViewerAnimatedScrollHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class ScrollViewerAnimatedScrollHandler : FrameworkElement
  {
    private ScrollViewer _scrollViewer;
    private Slider _sliderHorizontal;
    private Slider _sliderVertical;
    private Slider _sliderZoom;

    public void Attach(ScrollViewer scrollViewer)
    {
      this._scrollViewer = scrollViewer;
      this._sliderHorizontal = new Slider();
      ((RangeBase) this._sliderHorizontal).put_SmallChange(1E-10);
      ((RangeBase) this._sliderHorizontal).put_Minimum(double.MinValue);
      ((RangeBase) this._sliderHorizontal).put_Maximum(double.MaxValue);
      this._sliderHorizontal.put_StepFrequency(1E-10);
      Slider sliderHorizontal = this._sliderHorizontal;
      WindowsRuntimeMarshal.AddEventHandler<RangeBaseValueChangedEventHandler>((Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>) new Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>(((RangeBase) sliderHorizontal).add_ValueChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) sliderHorizontal).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnHorizontalOffsetChanged));
      this._sliderVertical = new Slider();
      ((RangeBase) this._sliderVertical).put_SmallChange(1E-10);
      ((RangeBase) this._sliderVertical).put_Minimum(double.MinValue);
      ((RangeBase) this._sliderVertical).put_Maximum(double.MaxValue);
      this._sliderVertical.put_StepFrequency(1E-10);
      Slider sliderVertical = this._sliderVertical;
      WindowsRuntimeMarshal.AddEventHandler<RangeBaseValueChangedEventHandler>((Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>) new Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>(((RangeBase) sliderVertical).add_ValueChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) sliderVertical).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnVerticalOffsetChanged));
      this._sliderZoom = new Slider();
      ((RangeBase) this._sliderZoom).put_SmallChange(1E-10);
      ((RangeBase) this._sliderZoom).put_Minimum(double.MinValue);
      ((RangeBase) this._sliderZoom).put_Maximum(double.MaxValue);
      this._sliderZoom.put_StepFrequency(1E-10);
      Slider sliderZoom = this._sliderZoom;
      WindowsRuntimeMarshal.AddEventHandler<RangeBaseValueChangedEventHandler>((Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>) new Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>(((RangeBase) sliderZoom).add_ValueChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) sliderZoom).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnZoomFactorChanged));
    }

    public void Detach()
    {
      this._scrollViewer = (ScrollViewer) null;
      if (this._sliderHorizontal != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RangeBaseValueChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) this._sliderHorizontal).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnHorizontalOffsetChanged));
        this._sliderHorizontal = (Slider) null;
      }
      if (this._sliderVertical != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RangeBaseValueChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) this._sliderVertical).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnHorizontalOffsetChanged));
        this._sliderVertical = (Slider) null;
      }
      if (this._sliderZoom == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<RangeBaseValueChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) this._sliderZoom).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnZoomFactorChanged));
      this._sliderZoom = (Slider) null;
    }

    private void OnHorizontalOffsetChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
      if (this._scrollViewer == null)
        return;
      this._scrollViewer.ChangeView((double?) new double?(e.NewValue), (double?) new double?(), (float?) new float?(), true);
    }

    private void OnVerticalOffsetChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
      if (this._scrollViewer == null)
        return;
      this._scrollViewer.ChangeView((double?) new double?(), (double?) new double?(e.NewValue), (float?) new float?(), true);
    }

    private void OnZoomFactorChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
      if (this._scrollViewer == null)
        return;
      this._scrollViewer.ChangeView((double?) new double?(), (double?) new double?(), (float?) new float?((float) e.NewValue), true);
    }

    internal async Task ScrollToHorizontalOffsetWithAnimation(
      double offset,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      Storyboard sb = new Storyboard();
      DoubleAnimation da = new DoubleAnimation();
      da.put_EnableDependentAnimation(true);
      da.put_From((double?) new double?(this._scrollViewer.HorizontalOffset));
      da.put_To((double?) new double?(offset));
      da.put_EasingFunction(easingFunction);
      ((Timeline) da).put_Duration((Duration) (TimeSpan) duration);
      ((ICollection<Timeline>) sb.Children).Add((Timeline) da);
      Storyboard.SetTarget((Timeline) sb, (DependencyObject) this._sliderHorizontal);
      Storyboard.SetTargetProperty((Timeline) da, "Value");
      await sb.BeginAsync();
    }

    internal async Task ScrollToVerticalOffsetWithAnimation(
      double offset,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      Storyboard sb = new Storyboard();
      DoubleAnimation da = new DoubleAnimation();
      da.put_EnableDependentAnimation(true);
      da.put_From((double?) new double?(this._scrollViewer.VerticalOffset));
      da.put_To((double?) new double?(offset));
      da.put_EasingFunction(easingFunction);
      ((Timeline) da).put_Duration((Duration) (TimeSpan) duration);
      ((ICollection<Timeline>) sb.Children).Add((Timeline) da);
      Storyboard.SetTarget((Timeline) sb, (DependencyObject) this._sliderVertical);
      Storyboard.SetTargetProperty((Timeline) da, "Value");
      await sb.BeginAsync();
    }

    internal async Task ZoomToFactorWithAnimation(
      double factor,
      TimeSpan duration,
      EasingFunctionBase easingFunction)
    {
      Storyboard sb = new Storyboard();
      DoubleAnimation da = new DoubleAnimation();
      da.put_EnableDependentAnimation(true);
      da.put_From((double?) new double?((double) this._scrollViewer.ZoomFactor));
      da.put_To((double?) new double?(factor));
      da.put_EasingFunction(easingFunction);
      ((Timeline) da).put_Duration((Duration) (TimeSpan) duration);
      ((ICollection<Timeline>) sb.Children).Add((Timeline) da);
      Storyboard.SetTarget((Timeline) sb, (DependencyObject) this._sliderZoom);
      Storyboard.SetTargetProperty((Timeline) da, "Value");
      await sb.BeginAsync();
    }
  }
}
