// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.PageTransitionAnimation
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
  public abstract class PageTransitionAnimation : DependencyObject
  {
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), (Type) typeof (AnimationMode), (Type) typeof (PageTransitionAnimation), new PropertyMetadata((object) AnimationMode.Out));
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), (Type) typeof (Duration), (Type) typeof (PageTransitionAnimation), new PropertyMetadata((object) new Duration((TimeSpan) TimeSpan.FromSeconds(0.4))));
    public static readonly DependencyProperty EasingFunctionProperty;

    protected abstract Storyboard Animation { get; }

    public AnimationMode Mode
    {
      get => (AnimationMode) this.GetValue(PageTransitionAnimation.ModeProperty);
      set => this.SetValue(PageTransitionAnimation.ModeProperty, (object) value);
    }

    public Duration Duration
    {
      get => (Duration) this.GetValue(PageTransitionAnimation.DurationProperty);
      set => this.SetValue(PageTransitionAnimation.DurationProperty, (object) value);
    }

    public EasingFunctionBase EasingFunction
    {
      get => (EasingFunctionBase) this.GetValue(PageTransitionAnimation.EasingFunctionProperty);
      set => this.SetValue(PageTransitionAnimation.EasingFunctionProperty, (object) value);
    }

    protected virtual void ApplyTargetProperties(DependencyObject target, Storyboard animation)
    {
    }

    internal virtual void CleanupAnimation(DependencyObject target, Storyboard animation)
    {
    }

    internal Storyboard GetAnimation(DependencyObject target)
    {
      Storyboard animation = this.Animation;
      Storyboard.SetTarget((Timeline) animation, target);
      this.ApplyTargetProperties(target, animation);
      return animation;
    }

    internal async Task Animate(DependencyObject target)
    {
      Storyboard anim = this.Animation;
      Storyboard.SetTarget((Timeline) anim, target);
      this.ApplyTargetProperties(target, anim);
      await anim.BeginAsync();
      anim.Stop();
      this.CleanupAnimation(target, anim);
    }

    static PageTransitionAnimation()
    {
      Type type1 = typeof (EasingFunctionBase);
      Type type2 = typeof (PageTransitionAnimation);
      CubicEase cubicEase = new CubicEase();
      ((EasingFunctionBase) cubicEase).put_EasingMode((EasingMode) 0);
      PropertyMetadata propertyMetadata = new PropertyMetadata((object) cubicEase);
      PageTransitionAnimation.EasingFunctionProperty = DependencyProperty.Register(nameof (EasingFunction), (Type) type1, (Type) type2, propertyMetadata);
    }
  }
}
