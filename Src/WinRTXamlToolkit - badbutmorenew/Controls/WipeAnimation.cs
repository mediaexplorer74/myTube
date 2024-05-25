// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.WipeAnimation
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
  public class WipeAnimation : PageTransitionAnimation
  {
    private Slider _slider;
    private FrameworkElement _fe;
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), (Type) typeof (DirectionOfMotion), (Type) typeof (WipeAnimation), new PropertyMetadata((object) DirectionOfMotion.RightToLeft));

    public DirectionOfMotion Direction
    {
      get => (DirectionOfMotion) this.GetValue(WipeAnimation.DirectionProperty);
      set => this.SetValue(WipeAnimation.DirectionProperty, (object) value);
    }

    protected override Storyboard Animation
    {
      get
      {
        Storyboard animation = new Storyboard();
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.put_EasingFunction(this.EasingFunction);
        ((Timeline) doubleAnimation).put_Duration(this.Duration);
        ((ICollection<Timeline>) animation.Children).Add((Timeline) doubleAnimation);
        return animation;
      }
    }

    protected override void ApplyTargetProperties(DependencyObject target, Storyboard animation)
    {
      this._fe = (FrameworkElement) target;
      if (((UIElement) this._fe).Clip == null)
        ((UIElement) this._fe).put_Clip(new RectangleGeometry());
      if (this.Mode == AnimationMode.Out)
      {
        ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, this._fe.ActualWidth, this._fe.ActualHeight));
      }
      else
      {
        switch (this.Direction)
        {
          case DirectionOfMotion.RightToLeft:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(this._fe.ActualHeight, 0.0, 0.0, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.LeftToRight:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, 0.0, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.TopToBottom:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, this._fe.ActualWidth, 0.0));
            break;
          case DirectionOfMotion.BottomToTop:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, this._fe.ActualHeight, this._fe.ActualWidth, 0.0));
            break;
        }
      }
      DoubleAnimation child = (DoubleAnimation) ((IList<Timeline>) animation.Children)[0];
      child.put_EnableDependentAnimation(true);
      if (this._slider == null)
      {
        this._slider = new Slider();
        ((RangeBase) this._slider).put_SmallChange(1E-10);
        ((RangeBase) this._slider).put_Minimum(double.MinValue);
        ((RangeBase) this._slider).put_Maximum(double.MaxValue);
        this._slider.put_StepFrequency(1E-10);
        Slider slider = this._slider;
        WindowsRuntimeMarshal.AddEventHandler<RangeBaseValueChangedEventHandler>((Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>) new Func<RangeBaseValueChangedEventHandler, EventRegistrationToken>(((RangeBase) slider).add_ValueChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) slider).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnSliderValueChanged));
      }
      Storyboard.SetTarget((Timeline) animation, (DependencyObject) this._slider);
      Storyboard.SetTargetProperty((Timeline) child, "Value");
      child.put_From((double?) new double?(0.0));
      child.put_To((double?) new double?(1.0));
    }

    private void OnSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
      double num = ((RangeBase) this._slider).Value;
      if (this.Mode == AnimationMode.Out)
      {
        switch (this.Direction)
        {
          case DirectionOfMotion.RightToLeft:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, num * this._fe.ActualWidth, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.LeftToRight:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(num * this._fe.ActualWidth, 0.0, (1.0 - num) * this._fe.ActualWidth, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.TopToBottom:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, num * this._fe.ActualHeight, this._fe.ActualWidth, (1.0 - num) * this._fe.ActualHeight));
            break;
          case DirectionOfMotion.BottomToTop:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, this._fe.ActualWidth, num * this._fe.ActualHeight));
            break;
        }
      }
      else
      {
        switch (this.Direction)
        {
          case DirectionOfMotion.RightToLeft:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect((1.0 - num) * this._fe.ActualWidth, 0.0, num * this._fe.ActualWidth, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.LeftToRight:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, num * this._fe.ActualWidth, this._fe.ActualHeight));
            break;
          case DirectionOfMotion.TopToBottom:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, 0.0, this._fe.ActualWidth, num * this._fe.ActualHeight));
            break;
          case DirectionOfMotion.BottomToTop:
            ((UIElement) this._fe).Clip.put_Rect((Rect) new Rect(0.0, (1.0 - num) * this._fe.ActualHeight, this._fe.ActualWidth, num * this._fe.ActualHeight));
            break;
        }
      }
    }

    internal override void CleanupAnimation(DependencyObject target, Storyboard animation)
    {
      base.CleanupAnimation(target, animation);
      WindowsRuntimeMarshal.RemoveEventHandler<RangeBaseValueChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((RangeBase) this._slider).remove_ValueChanged), new RangeBaseValueChangedEventHandler(this.OnSliderValueChanged));
      this._slider = (Slider) null;
      this._fe = (FrameworkElement) null;
    }
  }
}
