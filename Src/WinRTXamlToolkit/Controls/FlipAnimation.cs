// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FlipAnimation
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  public class FlipAnimation : PageTransitionAnimation
  {
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), (Type) typeof (DirectionOfMotion), (Type) typeof (FlipAnimation), new PropertyMetadata((object) DirectionOfMotion.RightToLeft));
    public static readonly DependencyProperty AxisOfFlipProperty = DependencyProperty.Register(nameof (AxisOfFlip), (Type) typeof (AxisOfFlip), (Type) typeof (FlipAnimation), new PropertyMetadata((object) AxisOfFlip.LeftOrTop));

    public DirectionOfMotion Direction
    {
      get => (DirectionOfMotion) this.GetValue(FlipAnimation.DirectionProperty);
      set => this.SetValue(FlipAnimation.DirectionProperty, (object) value);
    }

    public AxisOfFlip AxisOfFlip
    {
      get => (AxisOfFlip) this.GetValue(FlipAnimation.AxisOfFlipProperty);
      set => this.SetValue(FlipAnimation.AxisOfFlipProperty, (object) value);
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
        ObjectAnimationUsingKeyFrames animationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
        if (this.AxisOfFlip == AxisOfFlip.Center)
        {
          ObjectKeyFrameCollection keyFrames1 = animationUsingKeyFrames.KeyFrames;
          DiscreteObjectKeyFrame discreteObjectKeyFrame1 = new DiscreteObjectKeyFrame();
          ((ObjectKeyFrame) discreteObjectKeyFrame1).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) new TimeSpan(0L)));
          ((ObjectKeyFrame) discreteObjectKeyFrame1).put_Value((object) (Visibility) (this.Mode == AnimationMode.ForwardIn || this.Mode == AnimationMode.BackwardIn ? 1 : 0));
          DiscreteObjectKeyFrame discreteObjectKeyFrame2 = discreteObjectKeyFrame1;
          ((ICollection<ObjectKeyFrame>) keyFrames1).Add((ObjectKeyFrame) discreteObjectKeyFrame2);
          double num = 0.0;
          while (this.EasingFunction.Ease(num) < 0.5)
            num += 0.001;
          ObjectKeyFrameCollection keyFrames2 = animationUsingKeyFrames.KeyFrames;
          DiscreteObjectKeyFrame discreteObjectKeyFrame3 = new DiscreteObjectKeyFrame();
          ((ObjectKeyFrame) discreteObjectKeyFrame3).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(((TimeSpan) this.Duration.TimeSpan).TotalSeconds * num)));
          ((ObjectKeyFrame) discreteObjectKeyFrame3).put_Value((object) (Visibility) (this.Mode == AnimationMode.ForwardIn || this.Mode == AnimationMode.BackwardIn ? 0 : 1));
          DiscreteObjectKeyFrame discreteObjectKeyFrame4 = discreteObjectKeyFrame3;
          ((ICollection<ObjectKeyFrame>) keyFrames2).Add((ObjectKeyFrame) discreteObjectKeyFrame4);
        }
        ((ICollection<Timeline>) animation.Children).Add((Timeline) animationUsingKeyFrames);
        return animation;
      }
    }

    protected override void ApplyTargetProperties(DependencyObject target, Storyboard animation)
    {
      FrameworkElement frameworkElement = (FrameworkElement) target;
      if (!(((UIElement) frameworkElement).Projection is PlaneProjection planeProjection))
        ((UIElement) frameworkElement).put_Projection((Projection) (planeProjection = new PlaneProjection()));
      if (!(((UIElement) frameworkElement).RenderTransform is TranslateTransform translateTransform))
        ((UIElement) frameworkElement).put_RenderTransform((Transform) (translateTransform = new TranslateTransform()));
      if (this.Direction == DirectionOfMotion.TopToBottom || this.Direction == DirectionOfMotion.BottomToTop)
      {
        if (this.AxisOfFlip == AxisOfFlip.LeftOrTop)
        {
          planeProjection.put_CenterOfRotationY(0.0);
          planeProjection.put_GlobalOffsetY(frameworkElement.ActualHeight / 2.0);
          translateTransform.put_Y(-frameworkElement.ActualHeight / 2.0);
        }
        else if (this.AxisOfFlip == AxisOfFlip.RightOrBottom)
        {
          planeProjection.put_CenterOfRotationY(1.0);
          planeProjection.put_GlobalOffsetY(-frameworkElement.ActualHeight / 2.0);
          translateTransform.put_Y(frameworkElement.ActualHeight / 2.0);
        }
        else
          planeProjection.put_CenterOfRotationY(0.5);
      }
      else if (this.AxisOfFlip == AxisOfFlip.LeftOrTop)
      {
        planeProjection.put_CenterOfRotationX(0.0);
        planeProjection.put_GlobalOffsetX(frameworkElement.ActualWidth / 2.0);
        translateTransform.put_X(-frameworkElement.ActualWidth / 2.0);
      }
      else if (this.AxisOfFlip == AxisOfFlip.RightOrBottom)
      {
        planeProjection.put_CenterOfRotationX(1.0);
        planeProjection.put_GlobalOffsetX(-frameworkElement.ActualWidth / 2.0);
        translateTransform.put_X(frameworkElement.ActualWidth / 2.0);
      }
      else
        planeProjection.put_CenterOfRotationX(0.5);
      DoubleAnimation child1 = (DoubleAnimation) ((IList<Timeline>) animation.Children)[0];
      Storyboard.SetTarget((Timeline) child1, (DependencyObject) planeProjection);
      Storyboard.SetTargetProperty((Timeline) child1, this.Direction == DirectionOfMotion.TopToBottom || this.Direction == DirectionOfMotion.BottomToTop ? "RotationX" : "RotationY");
      ObjectAnimationUsingKeyFrames child2 = (ObjectAnimationUsingKeyFrames) ((IList<Timeline>) animation.Children)[1];
      Storyboard.SetTarget((Timeline) child2, (DependencyObject) frameworkElement);
      Storyboard.SetTargetProperty((Timeline) child2, "Visibility");
      if (this.AxisOfFlip == AxisOfFlip.Center)
      {
        if (this.Mode == AnimationMode.ForwardOut || this.Mode == AnimationMode.BackwardOut)
        {
          switch (this.Direction)
          {
            case DirectionOfMotion.RightToLeft:
              child1.put_From((double?) new double?(0.0));
              child1.put_To((double?) new double?(180.0));
              break;
            case DirectionOfMotion.LeftToRight:
              child1.put_From((double?) new double?(0.0));
              child1.put_To((double?) new double?(-180.0));
              break;
            case DirectionOfMotion.TopToBottom:
              child1.put_From((double?) new double?(0.0));
              child1.put_To((double?) new double?(180.0));
              break;
            case DirectionOfMotion.BottomToTop:
              child1.put_From((double?) new double?(0.0));
              child1.put_To((double?) new double?(-180.0));
              break;
          }
        }
        else
        {
          switch (this.Direction)
          {
            case DirectionOfMotion.RightToLeft:
              child1.put_From((double?) new double?(-180.0));
              child1.put_To((double?) new double?(0.0));
              break;
            case DirectionOfMotion.LeftToRight:
              child1.put_From((double?) new double?(180.0));
              child1.put_To((double?) new double?(0.0));
              break;
            case DirectionOfMotion.TopToBottom:
              child1.put_From((double?) new double?(180.0));
              child1.put_To((double?) new double?(0.0));
              break;
            case DirectionOfMotion.BottomToTop:
              child1.put_From((double?) new double?(180.0));
              child1.put_To((double?) new double?(0.0));
              break;
          }
        }
      }
      else
      {
        switch (this.Mode)
        {
          case AnimationMode.ForwardIn:
          case AnimationMode.BackwardOut:
            child1.put_From((double?) new double?(0.0));
            child1.put_To((double?) new double?(0.0));
            break;
          case AnimationMode.ForwardOut:
            switch (this.Direction)
            {
              case DirectionOfMotion.RightToLeft:
              case DirectionOfMotion.LeftToRight:
                child1.put_From((double?) new double?(0.0));
                child1.put_To((double?) new double?(this.AxisOfFlip == AxisOfFlip.LeftOrTop ? 90.0 : -90.0));
                return;
              case DirectionOfMotion.TopToBottom:
              case DirectionOfMotion.BottomToTop:
                child1.put_From((double?) new double?(0.0));
                child1.put_To((double?) new double?(this.AxisOfFlip == AxisOfFlip.LeftOrTop ? -90.0 : 90.0));
                return;
              default:
                return;
            }
          case AnimationMode.BackwardIn:
            switch (this.Direction)
            {
              case DirectionOfMotion.RightToLeft:
              case DirectionOfMotion.LeftToRight:
                child1.put_From((double?) new double?(this.AxisOfFlip == AxisOfFlip.LeftOrTop ? 90.0 : -90.0));
                child1.put_To((double?) new double?(0.0));
                return;
              case DirectionOfMotion.TopToBottom:
              case DirectionOfMotion.BottomToTop:
                child1.put_From((double?) new double?(this.AxisOfFlip == AxisOfFlip.LeftOrTop ? -90.0 : 90.0));
                child1.put_To((double?) new double?(0.0));
                return;
              default:
                return;
            }
        }
      }
    }

    internal override void CleanupAnimation(DependencyObject target, Storyboard animation)
    {
      base.CleanupAnimation(target, animation);
      FrameworkElement start = (FrameworkElement) target;
      ((UIElement) start).put_Projection((Projection) new PlaneProjection());
      ((UIElement) start).put_RenderTransform((Transform) new CompositeTransform());
      foreach (ScrollViewer scrollViewer in ((DependencyObject) start).GetDescendantsOfType<ScrollViewer>())
      {
        scrollViewer.put_ZoomMode((ZoomMode) ((scrollViewer.ZoomMode + 1) % 2));
        scrollViewer.put_ZoomMode((ZoomMode) ((scrollViewer.ZoomMode + 1) % 2));
      }
    }
  }
}
