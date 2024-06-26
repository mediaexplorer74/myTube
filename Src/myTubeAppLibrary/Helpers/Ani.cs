﻿// Type: myTube.Helpers.Ani

using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace myTube.Helpers
{
  public static class Ani
  {
    public static Storyboard Begin(
      DependencyObject Element,
      string Property,
      double To,
      double Duration,
      double Ease)
    {
      Storyboard storyboard = Ani.Animation(Ani.DoubleAni(Element, Property, To, Duration, Ease));
      storyboard.Begin();
      return storyboard;
    }

    public static Storyboard Begin(
      DependencyObject Element,
      string Property,
      double To,
      double Duration,
      EasingFunctionBase Ease)
    {
      Storyboard storyboard = Ani.Animation(Ani.DoubleAni(Element, Property, To, Duration, Ease));
      storyboard.Begin();
      return storyboard;
    }

    public static ColorAnimation ColorAni(
      DependencyObject target,
      Color from,
      Color to,
      double duration)
    {
      ColorAnimation colorAnimation = new ColorAnimation();
      ((Timeline) colorAnimation).Duration = (Duration) TimeSpan.FromSeconds(duration);
      Storyboard.SetTarget((Timeline) colorAnimation, target);
      Storyboard.SetTargetProperty((Timeline) colorAnimation, new PropertyPath("(UIElement.Background).(SolidColorBrush.Color)").Path);
      colorAnimation.From = new Color?(from);
      colorAnimation.To = new Color?(to);
      return colorAnimation;
    }

    public static ColorAnimation BackgroundColorAni(
      DependencyObject target,
      Color to,
      double duration)
    {
      ColorAnimation colorAnimation = new ColorAnimation();
      ((Timeline) colorAnimation).Duration = (Duration) TimeSpan.FromSeconds(duration);
      Storyboard.SetTarget((Timeline) colorAnimation, target);
      Storyboard.SetTargetProperty((Timeline) colorAnimation, new PropertyPath("(UIElement.Background).(SolidColorBrush.Color)").Path);
      colorAnimation.To = new Color?(to);
      return colorAnimation;
    }

    public static Storyboard Begin(DependencyObject target, Color to, double duration)
    {
      Storyboard storyboard = Ani.Animation(Ani.BackgroundColorAni(target, to, duration));
      storyboard.Begin();
      return storyboard;
    }

    public static Storyboard Begin(params Timeline[] dAni)
    {
      Storyboard sb = new Storyboard();
      sb.Add(dAni);
      sb.Begin();
      return sb;
    }

    public static Storyboard Begin(
      DependencyObject Element,
      string Property,
      double To,
      double Duration)
    {
      return Ani.Begin(Element, Property, To, Duration, 1.0);
    }

    public static DoubleAnimation DoubleAni(
      DependencyObject Element,
      string Property,
      double To,
      double Duration,
      double Ease)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) doubleAnimation, Element);
      Storyboard.SetTargetProperty((Timeline) doubleAnimation, Property);
      doubleAnimation.To = new double?(To);
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).EasingMode = (EasingMode) 2;
      exponentialEase.Exponent = Ease;
      doubleAnimation.EasingFunction = (EasingFunctionBase) exponentialEase;
      ((Timeline) doubleAnimation).Duration = (Duration) TimeSpan.FromSeconds(Duration);
      return doubleAnimation;
    }

    public static DoubleAnimation DoubleAni(
      DependencyObject Element,
      string Property,
      double To,
      double Duration,
      EasingFunctionBase Ease)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) doubleAnimation, Element);
      Storyboard.SetTargetProperty((Timeline) doubleAnimation, new PropertyPath(Property).Path);
      doubleAnimation.To = new double?(To);
      doubleAnimation.EasingFunction = Ease;
      ((Timeline) doubleAnimation).Duration = (Duration) TimeSpan.FromSeconds(Duration);
      return doubleAnimation;
    }

    public static DoubleAnimation DoubleAni(
      DependencyObject Element,
      string Property,
      double To,
      double Duration,
      EasingFunctionBase Ease,
      double startTime)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) doubleAnimation, Element);
      Storyboard.SetTargetProperty((Timeline) doubleAnimation, new PropertyPath(Property).Path);
      doubleAnimation.To = new double?(To);

      if (Ease != null)
        doubleAnimation.EasingFunction = Ease;

      ((Timeline) doubleAnimation).BeginTime = new TimeSpan?(TimeSpan.FromSeconds(startTime));
      ((Timeline) doubleAnimation).Duration = (Duration) TimeSpan.FromSeconds(Duration);
      return doubleAnimation;
    }

    public static ExponentialEase Ease(EasingMode Mode, double exponent)
    {
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).EasingMode = Mode;
      exponentialEase.Exponent = exponent;
      return exponentialEase;
    }

    public static Windows.UI.Xaml.Media.Animation.BounceEase BounceEase(
      EasingMode Mode,
      int bounces,
      double bounciness)
    {
      Windows.UI.Xaml.Media.Animation.BounceEase bounceEase = new Windows.UI.Xaml.Media.Animation.BounceEase();
      ((EasingFunctionBase) bounceEase).EasingMode = Mode;
      bounceEase.Bounces = bounces;
      bounceEase.Bounciness = bounciness;
      return bounceEase;
    }

    public static DoubleAnimation DoubleAni(
      DependencyObject Element,
      string Property,
      double To,
      double Duration)
    {
      return Ani.DoubleAni(Element, Property, To, Duration, 1.0);
    }

    public static Storyboard Animation(params DoubleAnimation[] dAni)
    {
      Storyboard sb = new Storyboard();
      sb.Add((Timeline[]) dAni);
      return sb;
    }

    public static Storyboard Animation(ColorAnimation cAni)
    {
      Storyboard storyboard = new Storyboard();
      ((Timeline) storyboard).Duration = ((Timeline) cAni).Duration;
      ((ICollection<Timeline>) storyboard.Children).Add((Timeline) cAni);
      return storyboard;
    }

    public static Storyboard Animation(double time)
    {
      Storyboard storyboard = new Storyboard();
      ((Timeline) storyboard).Duration = (Duration) TimeSpan.FromSeconds(time);
      return storyboard;
    }

    public static void Add(this Storyboard sb, Timeline dAni) => ((ICollection<Timeline>) sb.Children).Add(dAni);

    public static void Add(this Storyboard sb, params Timeline[] dAni)
    {
      foreach (Timeline timeline in dAni)
      {
        if (timeline != null)
          ((ICollection<Timeline>) sb.Children).Add(timeline);
      }
    }

    public static DoubleAnimation SetFrom(this DoubleAnimation da, double from)
    {
      da.From = new double?(from);
      return da;
    }
  }
}
