// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.PageTransition
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
  public abstract class PageTransition : DependencyObject
  {
    public static readonly DependencyProperty ForwardOutAnimationProperty = DependencyProperty.Register(nameof (ForwardOutAnimation), (Type) typeof (PageTransitionAnimation), (Type) typeof (PageTransition), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ForwardInAnimationProperty = DependencyProperty.Register(nameof (ForwardInAnimation), (Type) typeof (PageTransitionAnimation), (Type) typeof (PageTransition), new PropertyMetadata((object) null));
    public static readonly DependencyProperty BackwardOutAnimationProperty = DependencyProperty.Register(nameof (BackwardOutAnimation), (Type) typeof (PageTransitionAnimation), (Type) typeof (PageTransition), new PropertyMetadata((object) null));
    public static readonly DependencyProperty BackwardInAnimationProperty = DependencyProperty.Register(nameof (BackwardInAnimation), (Type) typeof (PageTransitionAnimation), (Type) typeof (PageTransition), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), (Type) typeof (Duration), (Type) typeof (PageTransition), new PropertyMetadata((object) new Duration((TimeSpan) TimeSpan.FromSeconds(0.4))));
    public static readonly DependencyProperty EasingFunctionProperty;

    protected abstract PageTransitionMode Mode { get; }

    public PageTransitionAnimation ForwardOutAnimation
    {
      get => (PageTransitionAnimation) this.GetValue(PageTransition.ForwardOutAnimationProperty);
      set => this.SetValue(PageTransition.ForwardOutAnimationProperty, (object) value);
    }

    public PageTransitionAnimation ForwardInAnimation
    {
      get => (PageTransitionAnimation) this.GetValue(PageTransition.ForwardInAnimationProperty);
      set => this.SetValue(PageTransition.ForwardInAnimationProperty, (object) value);
    }

    public PageTransitionAnimation BackwardOutAnimation
    {
      get => (PageTransitionAnimation) this.GetValue(PageTransition.BackwardOutAnimationProperty);
      set => this.SetValue(PageTransition.BackwardOutAnimationProperty, (object) value);
    }

    public PageTransitionAnimation BackwardInAnimation
    {
      get => (PageTransitionAnimation) this.GetValue(PageTransition.BackwardInAnimationProperty);
      set => this.SetValue(PageTransition.BackwardInAnimationProperty, (object) value);
    }

    public Duration Duration
    {
      get => (Duration) this.GetValue(PageTransition.DurationProperty);
      set => this.SetValue(PageTransition.DurationProperty, (object) value);
    }

    public EasingFunctionBase EasingFunction
    {
      get => (EasingFunctionBase) this.GetValue(PageTransition.EasingFunctionProperty);
      set => this.SetValue(PageTransition.EasingFunctionProperty, (object) value);
    }

    public async Task TransitionForward(DependencyObject previousPage, DependencyObject newPage)
    {
      if (previousPage == null && newPage == null)
        throw new ArgumentNullException(nameof (newPage));
      this.PrepareForwardAnimations(previousPage, newPage);
      this.UpdateTimelineAttributes();
      if (previousPage == null)
      {
        if (this.ForwardInAnimation != null)
          await this.ForwardInAnimation.Animate(newPage);
      }
      else if (newPage == null)
      {
        if (this.ForwardOutAnimation != null)
          await this.ForwardOutAnimation.Animate(previousPage);
      }
      else if (this.Mode == PageTransitionMode.Parallel)
      {
        Storyboard sb = new Storyboard();
        Storyboard outSb = (Storyboard) null;
        Storyboard inSb = (Storyboard) null;
        if (this.ForwardOutAnimation != null)
        {
          outSb = this.ForwardOutAnimation.GetAnimation(previousPage);
          ((ICollection<Timeline>) sb.Children).Add((Timeline) outSb);
        }
        if (this.ForwardInAnimation != null)
        {
          inSb = this.ForwardInAnimation.GetAnimation(newPage);
          ((ICollection<Timeline>) sb.Children).Add((Timeline) inSb);
        }
        await sb.BeginAsync();
        sb.Stop();
        ((ICollection<Timeline>) sb.Children).Clear();
        if (this.ForwardOutAnimation != null)
          this.ForwardOutAnimation.CleanupAnimation(previousPage, outSb);
        if (this.ForwardInAnimation != null)
          this.ForwardInAnimation.CleanupAnimation(newPage, inSb);
      }
      else
      {
        if (this.ForwardOutAnimation != null)
          await this.ForwardOutAnimation.Animate(previousPage);
        if (this.ForwardInAnimation != null)
          await this.ForwardInAnimation.Animate(newPage);
      }
      this.CleanupForwardAnimations(previousPage, newPage);
    }

    public async Task TransitionBackward(DependencyObject previousPage, DependencyObject newPage)
    {
      if (previousPage == null && newPage == null)
        throw new ArgumentNullException(nameof (newPage));
      this.PrepareBackwardAnimations(previousPage, newPage);
      this.UpdateTimelineAttributes();
      if (previousPage == null)
        await this.BackwardInAnimation.Animate(newPage);
      else if (newPage == null)
        await this.BackwardOutAnimation.Animate(previousPage);
      else if (this.Mode == PageTransitionMode.Parallel)
      {
        Storyboard sb = new Storyboard();
        Storyboard outSb = (Storyboard) null;
        Storyboard inSb = (Storyboard) null;
        if (this.BackwardOutAnimation != null)
        {
          outSb = this.BackwardOutAnimation.GetAnimation(previousPage);
          ((ICollection<Timeline>) sb.Children).Add((Timeline) outSb);
        }
        if (this.BackwardInAnimation != null)
        {
          inSb = this.BackwardInAnimation.GetAnimation(newPage);
          ((ICollection<Timeline>) sb.Children).Add((Timeline) inSb);
        }
        await sb.BeginAsync();
        sb.Stop();
        ((ICollection<Timeline>) sb.Children).Clear();
        if (this.BackwardOutAnimation != null)
          this.BackwardOutAnimation.CleanupAnimation(previousPage, outSb);
        if (this.BackwardInAnimation != null)
          this.BackwardInAnimation.CleanupAnimation(newPage, inSb);
      }
      else
      {
        if (this.BackwardOutAnimation != null)
          await this.BackwardOutAnimation.Animate(previousPage);
        if (this.BackwardInAnimation != null)
          await this.BackwardInAnimation.Animate(newPage);
      }
      this.CleanupBackwardAnimations(previousPage, newPage);
    }

    protected virtual void UpdateTimelineAttributes()
    {
      if (this.Mode == PageTransitionMode.Parallel)
      {
        if (this.ForwardInAnimation != null)
        {
          this.ForwardInAnimation.Duration = this.Duration;
          this.ForwardInAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.ForwardOutAnimation != null)
        {
          this.ForwardOutAnimation.Duration = this.Duration;
          this.ForwardOutAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.BackwardInAnimation != null)
        {
          this.BackwardInAnimation.Duration = this.Duration;
          this.BackwardInAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.BackwardOutAnimation == null)
          return;
        this.BackwardOutAnimation.Duration = this.Duration;
        this.BackwardOutAnimation.EasingFunction = this.EasingFunction;
      }
      else
      {
        if (this.ForwardInAnimation != null)
        {
          this.ForwardInAnimation.Duration = (Duration) (TimeSpan) TimeSpan.FromSeconds(((TimeSpan) this.Duration.TimeSpan).TotalSeconds * 0.5);
          this.ForwardInAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.ForwardOutAnimation != null)
        {
          this.ForwardOutAnimation.Duration = (Duration) (TimeSpan) TimeSpan.FromSeconds(((TimeSpan) this.Duration.TimeSpan).TotalSeconds * 0.5);
          this.ForwardOutAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.BackwardInAnimation != null)
        {
          this.BackwardInAnimation.Duration = (Duration) (TimeSpan) TimeSpan.FromSeconds(((TimeSpan) this.Duration.TimeSpan).TotalSeconds * 0.5);
          this.BackwardInAnimation.EasingFunction = this.EasingFunction;
        }
        if (this.BackwardOutAnimation == null)
          return;
        this.BackwardOutAnimation.Duration = (Duration) (TimeSpan) TimeSpan.FromSeconds(((TimeSpan) this.Duration.TimeSpan).TotalSeconds * 0.5);
        this.BackwardOutAnimation.EasingFunction = this.EasingFunction;
      }
    }

    protected virtual void PrepareForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
    }

    protected virtual void PrepareBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
    }

    protected virtual void CleanupForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
    }

    protected virtual void CleanupBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
    }

    static PageTransition()
    {
      Type type1 = typeof (EasingFunctionBase);
      Type type2 = typeof (PageTransition);
      CubicEase cubicEase = new CubicEase();
      ((EasingFunctionBase) cubicEase).put_EasingMode((EasingMode) 0);
      PropertyMetadata propertyMetadata = new PropertyMetadata((object) cubicEase);
      PageTransition.EasingFunctionProperty = DependencyProperty.Register(nameof (EasingFunction), (Type) type1, (Type) type2, propertyMetadata);
    }
  }
}
