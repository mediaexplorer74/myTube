// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.Ani
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

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
      ((Timeline) colorAnimation).put_Duration((Duration) TimeSpan.FromSeconds(duration));
      Storyboard.SetTarget((Timeline) colorAnimation, target);
      Storyboard.SetTargetProperty((Timeline) colorAnimation, new PropertyPath("(UIElement.Background).(SolidColorBrush.Color)").Path);
      colorAnimation.put_From(new Color?(from));
      colorAnimation.put_To(new Color?(to));
      return colorAnimation;
    }

    public static ColorAnimation BackgroundColorAni(
      DependencyObject target,
      Color to,
      double duration)
    {
      ColorAnimation colorAnimation = new ColorAnimation();
      ((Timeline) colorAnimation).put_Duration((Duration) TimeSpan.FromSeconds(duration));
      Storyboard.SetTarget((Timeline) colorAnimation, target);
      Storyboard.SetTargetProperty((Timeline) colorAnimation, new PropertyPath("(UIElement.Background).(SolidColorBrush.Color)").Path);
      colorAnimation.put_To(new Color?(to));
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
      doubleAnimation.put_To(new double?(To));
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).put_EasingMode((EasingMode) 2);
      exponentialEase.put_Exponent(Ease);
      doubleAnimation.put_EasingFunction((EasingFunctionBase) exponentialEase);
      ((Timeline) doubleAnimation).put_Duration((Duration) TimeSpan.FromSeconds(Duration));
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
      doubleAnimation.put_To(new double?(To));
      doubleAnimation.put_EasingFunction(Ease);
      ((Timeline) doubleAnimation).put_Duration((Duration) TimeSpan.FromSeconds(Duration));
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
      doubleAnimation.put_To(new double?(To));
      if (Ease != null)
        doubleAnimation.put_EasingFunction(Ease);
      ((Timeline) doubleAnimation).put_BeginTime(new TimeSpan?(TimeSpan.FromSeconds(startTime)));
      ((Timeline) doubleAnimation).put_Duration((Duration) TimeSpan.FromSeconds(Duration));
      return doubleAnimation;
    }

    public static ExponentialEase Ease(EasingMode Mode, double exponent)
    {
      ExponentialEase exponentialEase = new ExponentialEase();
      ((EasingFunctionBase) exponentialEase).put_EasingMode(Mode);
      exponentialEase.put_Exponent(exponent);
      return exponentialEase;
    }

    public static Windows.UI.Xaml.Media.Animation.BounceEase BounceEase(
      EasingMode Mode,
      int bounces,
      double bounciness)
    {
      Windows.UI.Xaml.Media.Animation.BounceEase bounceEase = new Windows.UI.Xaml.Media.Animation.BounceEase();
      ((EasingFunctionBase) bounceEase).put_EasingMode(Mode);
      bounceEase.put_Bounces(bounces);
      bounceEase.put_Bounciness(bounciness);
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
      ((Timeline) storyboard).put_Duration(((Timeline) cAni).Duration);
      ((ICollection<Timeline>) storyboard.Children).Add((Timeline) cAni);
      return storyboard;
    }

    public static Storyboard Animation(double time)
    {
      Storyboard storyboard = new Storyboard();
      ((Timeline) storyboard).put_Duration((Duration) TimeSpan.FromSeconds(time));
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
      da.put_From(new double?(from));
      return da;
    }
  }
}
