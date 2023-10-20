// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.WipeTransition
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
  public class WipeTransition : PageTransition
  {
    private readonly Random _random = new Random();
    public static readonly DependencyProperty ForwardDirectionProperty = DependencyProperty.Register(nameof (ForwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (WipeTransition), new PropertyMetadata((object) DirectionOfMotion.RightToLeft, new PropertyChangedCallback(WipeTransition.OnForwardDirectionChanged)));
    public static readonly DependencyProperty BackwardDirectionProperty = DependencyProperty.Register(nameof (BackwardDirection), (Type) typeof (DirectionOfMotion), (Type) typeof (WipeTransition), new PropertyMetadata((object) DirectionOfMotion.LeftToRight, new PropertyChangedCallback(WipeTransition.OnBackwardDirectionChanged)));

    protected override PageTransitionMode Mode => PageTransitionMode.Parallel;

    public DirectionOfMotion ForwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(WipeTransition.ForwardDirectionProperty);
      set => this.SetValue(WipeTransition.ForwardDirectionProperty, (object) value);
    }

    private static void OnForwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      WipeTransition wipeTransition = (WipeTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion forwardDirection = wipeTransition.ForwardDirection;
      wipeTransition.OnForwardDirectionChanged(oldValue, forwardDirection);
    }

    protected virtual void OnForwardDirectionChanged(
      DirectionOfMotion oldForwardDirection,
      DirectionOfMotion newForwardDirection)
    {
      if (this.ForwardInAnimation != null)
        ((WipeAnimation) this.ForwardInAnimation).Direction = newForwardDirection;
      if (this.ForwardOutAnimation == null)
        return;
      ((WipeAnimation) this.ForwardOutAnimation).Direction = newForwardDirection;
    }

    public DirectionOfMotion BackwardDirection
    {
      get => (DirectionOfMotion) this.GetValue(WipeTransition.BackwardDirectionProperty);
      set => this.SetValue(WipeTransition.BackwardDirectionProperty, (object) value);
    }

    private static void OnBackwardDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      WipeTransition wipeTransition = (WipeTransition) d;
      DirectionOfMotion oldValue = (DirectionOfMotion) e.OldValue;
      DirectionOfMotion backwardDirection = wipeTransition.BackwardDirection;
      wipeTransition.OnBackwardDirectionChanged(oldValue, backwardDirection);
    }

    protected virtual void OnBackwardDirectionChanged(
      DirectionOfMotion oldBackwardDirection,
      DirectionOfMotion newBackwardDirection)
    {
      if (this.BackwardInAnimation != null)
        ((WipeAnimation) this.BackwardInAnimation).Direction = newBackwardDirection;
      if (this.BackwardOutAnimation == null)
        return;
      ((WipeAnimation) this.BackwardOutAnimation).Direction = newBackwardDirection;
    }

    public WipeTransition()
    {
      this.ForwardOutAnimation = (PageTransitionAnimation) null;
      WipeAnimation wipeAnimation1 = new WipeAnimation();
      wipeAnimation1.Direction = this.ForwardDirection;
      wipeAnimation1.Mode = AnimationMode.In;
      this.ForwardInAnimation = (PageTransitionAnimation) wipeAnimation1;
      this.BackwardOutAnimation = (PageTransitionAnimation) null;
      WipeAnimation wipeAnimation2 = new WipeAnimation();
      wipeAnimation2.Direction = this.BackwardDirection;
      wipeAnimation2.Mode = AnimationMode.In;
      this.BackwardInAnimation = (PageTransitionAnimation) wipeAnimation2;
    }

    protected override void PrepareForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareForwardAnimations(previousPage, newPage);
      Canvas.SetZIndex((UIElement) newPage, 1);
      if (this.ForwardDirection != DirectionOfMotion.Random)
        return;
      DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
      if (!(this.ForwardInAnimation is WipeAnimation))
        return;
      ((WipeAnimation) this.ForwardInAnimation).Direction = directionOfMotion;
    }

    protected override void PrepareBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      base.PrepareBackwardAnimations(previousPage, newPage);
      Canvas.SetZIndex((UIElement) newPage, 1);
      if (this.BackwardDirection != DirectionOfMotion.Random)
        return;
      DirectionOfMotion directionOfMotion = (DirectionOfMotion) this._random.Next(4);
      if (!(this.BackwardInAnimation is WipeAnimation))
        return;
      ((WipeAnimation) this.BackwardInAnimation).Direction = directionOfMotion;
    }

    protected override void CleanupBackwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      newPage.ClearValue(Canvas.ZIndexProperty);
      base.CleanupBackwardAnimations(previousPage, newPage);
      ((UIElement) newPage).put_Clip((RectangleGeometry) null);
    }

    protected override void CleanupForwardAnimations(
      DependencyObject previousPage,
      DependencyObject newPage)
    {
      newPage.ClearValue(Canvas.ZIndexProperty);
      base.CleanupForwardAnimations(previousPage, newPage);
      ((UIElement) newPage).put_Clip((RectangleGeometry) null);
    }
  }
}
