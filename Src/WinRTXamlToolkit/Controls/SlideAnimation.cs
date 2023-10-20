// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.SlideAnimation
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
  public class SlideAnimation : PageTransitionAnimation
  {
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), (Type) typeof (DirectionOfMotion), (Type) typeof (SlideAnimation), new PropertyMetadata((object) DirectionOfMotion.RightToLeft));

    public DirectionOfMotion Direction
    {
      get => (DirectionOfMotion) this.GetValue(SlideAnimation.DirectionProperty);
      set => this.SetValue(SlideAnimation.DirectionProperty, (object) value);
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
      FrameworkElement frameworkElement = (FrameworkElement) target;
      if (!(((UIElement) frameworkElement).RenderTransform is TranslateTransform translateTransform))
        ((UIElement) frameworkElement).put_RenderTransform((Transform) (translateTransform = new TranslateTransform()));
      DoubleAnimation child = (DoubleAnimation) ((IList<Timeline>) animation.Children)[0];
      Storyboard.SetTarget((Timeline) child, (DependencyObject) translateTransform);
      if (this.Direction == DirectionOfMotion.RightToLeft || this.Direction == DirectionOfMotion.LeftToRight)
      {
        Storyboard.SetTargetProperty((Timeline) child, "X");
        if (this.Mode == AnimationMode.In)
        {
          child.put_From((double?) new double?(this.Direction == DirectionOfMotion.LeftToRight ? -frameworkElement.ActualWidth : frameworkElement.ActualWidth));
          child.put_To((double?) new double?(0.0));
        }
        else
        {
          child.put_From((double?) new double?(0.0));
          child.put_To((double?) new double?(this.Direction == DirectionOfMotion.LeftToRight ? frameworkElement.ActualWidth : -frameworkElement.ActualWidth));
        }
      }
      else
      {
        Storyboard.SetTargetProperty((Timeline) child, "Y");
        if (this.Mode == AnimationMode.In)
        {
          child.put_From((double?) new double?(this.Direction == DirectionOfMotion.TopToBottom ? -frameworkElement.ActualHeight : frameworkElement.ActualHeight));
          child.put_To((double?) new double?(0.0));
        }
        else
        {
          child.put_From((double?) new double?(0.0));
          child.put_To((double?) new double?(this.Direction == DirectionOfMotion.TopToBottom ? frameworkElement.ActualHeight : -frameworkElement.ActualHeight));
        }
      }
    }
  }
}
