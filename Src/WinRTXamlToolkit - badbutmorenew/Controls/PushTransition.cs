// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.PushTransition
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
  public class PushTransition : PageTransition
  {
    private readonly Random _random = new Random();
    public static readonly DependencyProperty ForwardDirectionProperty = DependencyProperty.Register(nameof (ForwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (PushTransition), new PropertyMetadata((object) DirectionOfMotion.RightToLeft, new PropertyChangedCallback(PushTransition.OnForwardDirectionChanged)));
    public static readonly DependencyProperty BackwardDirectionProperty = DependencyProperty.Register(nameof (BackwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (PushTransition), new PropertyMetadata((object) DirectionOfMotion.LeftToRight, new PropertyChangedCallback(PushTransition.OnBackwardDirectionChanged)));

    protected override PageTransitionMode Mode => PageTransitionMode.Parallel;

    public DirectionOfMotion ForwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(PushTransition.ForwardDirectionProperty);
      set => this.SetValue(PushTransition.ForwardDirectionProperty, (object) value);
    }

    private static void OnForwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PushTransition pushTransition = (PushTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion forwardDirection = pushTransition.ForwardDirection;
      pushTransition.OnForwardDirectionChanged(oldValue, forwardDirection);
    }

    protected virtual void OnForwardDirectionChanged(
      DirectionOfMotion oldForwardDirection,
      DirectionOfMotion newForwardDirection)
    {
      if (this.ForwardInAnimation != null)
        ((SlideAnimation) this.ForwardInAnimation).Direction = newForwardDirection;
      if (this.ForwardOutAnimation == null)
        return;
      ((SlideAnimation) this.ForwardOutAnimation).Direction = newForwardDirection;
    }

    public DirectionOfMotion BackwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(PushTransition.BackwardDirectionProperty);
      set => this.SetValue(PushTransition.BackwardDirectionProperty, (object) value);
    }

    private static void OnBackwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PushTransition pushTransition = (PushTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion backwardDirection = pushTransition.BackwardDirection;
      pushTransition.OnBackwardDirectionChanged(oldValue, backwardDirection);
    }

    protected virtual void OnBackwardDirectionChanged(
      DirectionOfMotion oldBackwardDirection,
      DirectionOfMotion newBackwardDirection)
    {
      if (this.BackwardInAnimation != null)
        ((SlideAnimation) this.BackwardInAnimation).Direction = newBackwardDirection;
      if (this.BackwardOutAnimation == null)
        return;
      ((SlideAnimation) this.BackwardOutAnimation).Direction = newBackwardDirection;
    }

    public PushTransition()
    {
      SlideAnimation slideAnimation1 = new SlideAnimation();
      slideAnimation1.Direction = this.ForwardDirection;
      slideAnimation1.Mode = AnimationMode.Out;
      this.ForwardOutAnimation = (PageTransitionAnimation) slideAnimation1;
      SlideAnimation slideAnimation2 = new SlideAnimation();
      slideAnimation2.Direction = this.ForwardDirection;
      slideAnimation2.Mode = AnimationMode.In;
      this.ForwardInAnimation = (PageTransitionAnimation) slideAnimation2;
      SlideAnimation slideAnimation3 = new SlideAnimation();
      slideAnimation3.Direction = this.BackwardDirection;
      slideAnimation3.Mode = AnimationMode.Out;
      this.BackwardOutAnimation = (PageTransitionAnimation) slideAnimation3;
      SlideAnimation slideAnimation4 = new SlideAnimation();
      slideAnimation4.Direction = this.BackwardDirection;
      slideAnimation4.Mode = AnimationMode.In;
      this.BackwardInAnimation = (PageTransitionAnimation) slideAnimation4;
    }

    protected override void PrepareForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareForwardAnimations(previousPage, newPage);
      if (this.ForwardDirection != DirectionOfMotion.Random)
        return;
      DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
      if (this.ForwardOutAnimation is SlideAnimation)
        ((SlideAnimation) this.ForwardOutAnimation).Direction = directionOfMotion;
      if (!(this.ForwardInAnimation is SlideAnimation))
        return;
      ((SlideAnimation) this.ForwardInAnimation).Direction = directionOfMotion;
    }

    protected override void PrepareBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareBackwardAnimations(previousPage, newPage);
      if (this.BackwardDirection != DirectionOfMotion.Random)
        return;
      DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
      if (this.BackwardOutAnimation is SlideAnimation)
        ((SlideAnimation) this.BackwardOutAnimation).Direction = directionOfMotion;
      if (!(this.BackwardInAnimation is SlideAnimation))
        return;
      ((SlideAnimation) this.BackwardInAnimation).Direction = directionOfMotion;
    }

    protected override void CleanupBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.CleanupBackwardAnimations(previousPage, newPage);
    }

    protected override void CleanupForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.CleanupForwardAnimations(previousPage, newPage);
    }
  }
}
