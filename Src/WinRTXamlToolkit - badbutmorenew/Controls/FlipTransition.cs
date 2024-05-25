// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FlipTransition
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  public class FlipTransition : PageTransition
  {
    private readonly Random _random = new Random();
    public static readonly DependencyProperty ForwardDirectionProperty = DependencyProperty.Register(nameof (ForwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (FlipTransition), new PropertyMetadata((object) DirectionOfMotion.RightToLeft, new PropertyChangedCallback(FlipTransition.OnForwardDirectionChanged)));
    public static readonly DependencyProperty BackwardDirectionProperty = DependencyProperty.Register(nameof (BackwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (FlipTransition), new PropertyMetadata((object) DirectionOfMotion.LeftToRight, new PropertyChangedCallback(FlipTransition.OnBackwardDirectionChanged)));
    public static readonly DependencyProperty AxisOfFlipProperty = DependencyProperty.Register(nameof (AxisOfFlip), (Type) typeof (AxisOfFlip), (Type) typeof (FlipTransition), new PropertyMetadata((object) AxisOfFlip.LeftOrTop, new PropertyChangedCallback(FlipTransition.OnAxisOfFlipChanged)));

    protected override PageTransitionMode Mode => PageTransitionMode.Parallel;

    public DirectionOfMotion ForwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(FlipTransition.ForwardDirectionProperty);
      set => this.SetValue(FlipTransition.ForwardDirectionProperty, (object) value);
    }

    private static void OnForwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FlipTransition flipTransition = (FlipTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion forwardDirection = flipTransition.ForwardDirection;
      flipTransition.OnForwardDirectionChanged(oldValue, forwardDirection);
    }

    protected virtual void OnForwardDirectionChanged(
      DirectionOfMotion oldForwardDirection,
      DirectionOfMotion newForwardDirection)
    {
      if (this.ForwardInAnimation != null)
        ((FlipAnimation) this.ForwardInAnimation).Direction = newForwardDirection;
      if (this.ForwardOutAnimation == null)
        return;
      ((FlipAnimation) this.ForwardOutAnimation).Direction = newForwardDirection;
    }

    public DirectionOfMotion BackwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(FlipTransition.BackwardDirectionProperty);
      set => this.SetValue(FlipTransition.BackwardDirectionProperty, (object) value);
    }

    private static void OnBackwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FlipTransition flipTransition = (FlipTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion backwardDirection = flipTransition.BackwardDirection;
      flipTransition.OnBackwardDirectionChanged(oldValue, backwardDirection);
    }

    protected virtual void OnBackwardDirectionChanged(
      DirectionOfMotion oldBackwardDirection,
      DirectionOfMotion newBackwardDirection)
    {
      if (this.BackwardInAnimation != null)
        ((FlipAnimation) this.BackwardInAnimation).Direction = newBackwardDirection;
      if (this.BackwardOutAnimation == null)
        return;
      ((FlipAnimation) this.BackwardOutAnimation).Direction = newBackwardDirection;
    }

    public AxisOfFlip AxisOfFlip
    {
      get => (AxisOfFlip) this.GetValue(FlipTransition.AxisOfFlipProperty);
      set => this.SetValue(FlipTransition.AxisOfFlipProperty, (object) value);
    }

    private static void OnAxisOfFlipChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FlipTransition flipTransition = (FlipTransition) d;
      AxisOfFlip oldValue = (AxisOfFlip) e.OldValue;
      AxisOfFlip axisOfFlip = flipTransition.AxisOfFlip;
      flipTransition.OnAxisOfFlipChanged(oldValue, axisOfFlip);
    }

    private void OnAxisOfFlipChanged(AxisOfFlip oldAxisOfFlip, AxisOfFlip newAxisOfFlip)
    {
      if (this.ForwardInAnimation != null)
        ((FlipAnimation) this.ForwardInAnimation).AxisOfFlip = newAxisOfFlip;
      if (this.ForwardOutAnimation != null)
        ((FlipAnimation) this.ForwardOutAnimation).AxisOfFlip = newAxisOfFlip;
      if (this.BackwardInAnimation != null)
        ((FlipAnimation) this.BackwardInAnimation).AxisOfFlip = newAxisOfFlip;
      if (this.BackwardOutAnimation == null)
        return;
      ((FlipAnimation) this.BackwardOutAnimation).AxisOfFlip = newAxisOfFlip;
    }

    public FlipTransition()
    {
      FlipAnimation flipAnimation1 = new FlipAnimation();
      flipAnimation1.Direction = this.ForwardDirection;
      flipAnimation1.Mode = AnimationMode.ForwardOut;
      flipAnimation1.AxisOfFlip = this.AxisOfFlip;
      this.ForwardOutAnimation = (PageTransitionAnimation) flipAnimation1;
      FlipAnimation flipAnimation2 = new FlipAnimation();
      flipAnimation2.Direction = this.ForwardDirection;
      flipAnimation2.Mode = AnimationMode.ForwardIn;
      flipAnimation2.AxisOfFlip = this.AxisOfFlip;
      this.ForwardInAnimation = (PageTransitionAnimation) flipAnimation2;
      FlipAnimation flipAnimation3 = new FlipAnimation();
      flipAnimation3.Direction = this.BackwardDirection;
      flipAnimation3.Mode = AnimationMode.BackwardOut;
      flipAnimation3.AxisOfFlip = this.AxisOfFlip;
      this.BackwardOutAnimation = (PageTransitionAnimation) flipAnimation3;
      FlipAnimation flipAnimation4 = new FlipAnimation();
      flipAnimation4.Direction = this.BackwardDirection;
      flipAnimation4.Mode = AnimationMode.BackwardIn;
      flipAnimation4.AxisOfFlip = this.AxisOfFlip;
      this.BackwardInAnimation = (PageTransitionAnimation) flipAnimation4;
    }

    protected override void PrepareForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareForwardAnimations(previousPage, newPage);
      Canvas.SetZIndex((UIElement) newPage, -1);
      if (this.ForwardDirection == DirectionOfMotion.Random)
      {
        DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
        if (this.ForwardOutAnimation is FlipAnimation)
          ((FlipAnimation) this.ForwardOutAnimation).Direction = directionOfMotion;
        if (this.ForwardInAnimation is FlipAnimation)
          ((FlipAnimation) this.ForwardInAnimation).Direction = directionOfMotion;
      }
      if (this.AxisOfFlip != AxisOfFlip.Random)
        return;
      AxisOfFlip axisOfFlip = (AxisOfFlip) this._random.Next(3);
      if (this.ForwardOutAnimation is FlipAnimation)
        ((FlipAnimation) this.ForwardOutAnimation).AxisOfFlip = axisOfFlip;
      if (!(this.ForwardInAnimation is FlipAnimation))
        return;
      ((FlipAnimation) this.ForwardInAnimation).AxisOfFlip = axisOfFlip;
    }

    protected override void CleanupForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.CleanupForwardAnimations(previousPage, newPage);
      newPage.ClearValue(Canvas.ZIndexProperty);
    }

    protected override void PrepareBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareBackwardAnimations(previousPage, newPage);
      if (this.BackwardDirection == DirectionOfMotion.Random)
      {
        DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
        if (this.BackwardOutAnimation is FlipAnimation)
          ((FlipAnimation) this.BackwardOutAnimation).Direction = directionOfMotion;
        if (this.BackwardInAnimation is FlipAnimation)
          ((FlipAnimation) this.BackwardInAnimation).Direction = directionOfMotion;
      }
      if (this.AxisOfFlip != AxisOfFlip.Random)
        return;
      AxisOfFlip axisOfFlip = (AxisOfFlip) this._random.Next(3);
      if (this.BackwardOutAnimation is FlipAnimation)
        ((FlipAnimation) this.BackwardOutAnimation).AxisOfFlip = axisOfFlip;
      if (!(this.BackwardInAnimation is FlipAnimation))
        return;
      ((FlipAnimation) this.BackwardInAnimation).AxisOfFlip = axisOfFlip;
    }
  }
}
