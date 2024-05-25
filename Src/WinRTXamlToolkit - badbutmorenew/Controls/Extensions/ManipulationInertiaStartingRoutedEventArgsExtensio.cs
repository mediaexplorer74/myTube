// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ManipulationInertiaStartingRoutedEventArgsExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ManipulationInertiaStartingRoutedEventArgsExtensions
  {
    public const double DefaultDeceleration = 0.0036234234;

    public static void SetDesiredDisplacementX(
      this ManipulationInertiaStartingRoutedEventArgs e,
      double desiredDisplacementX)
    {
      double x = ((Point) e.Velocities.Linear).X;
      double y = ((Point) e.Velocities.Linear).Y;
      if (x == 0.0)
      {
        e.TranslationBehavior.put_DesiredDisplacement(desiredDisplacementX);
      }
      else
      {
        double num = Math.Sqrt(x * x + y * y);
        e.TranslationBehavior.put_DesiredDisplacement(desiredDisplacementX * num / x);
      }
    }

    public static void SetDesiredDisplacementY(
      this ManipulationInertiaStartingRoutedEventArgs e,
      double desiredDisplacementY)
    {
      double x = ((Point) e.Velocities.Linear).X;
      double y = ((Point) e.Velocities.Linear).Y;
      if (y == 0.0)
      {
        e.TranslationBehavior.put_DesiredDisplacement(desiredDisplacementY);
      }
      else
      {
        double num = Math.Sqrt(x * x + y * y);
        e.TranslationBehavior.put_DesiredDisplacement(desiredDisplacementY * num / y);
      }
    }

    public static double GetExpectedDisplacementDuration(
      this ManipulationInertiaStartingRoutedEventArgs e)
    {
      double desiredDisplacement = e.TranslationBehavior.DesiredDisplacement;
      double x = ((Point) e.Velocities.Linear).X;
      double y = ((Point) e.Velocities.Linear).Y;
      double num = Math.Sqrt(x * x + y * y);
      double d;
      if (double.IsNaN(desiredDisplacement))
      {
        d = e.TranslationBehavior.DesiredDeceleration;
        if (double.IsNaN(d))
          d = 0.0036234234;
      }
      else
        d = num * num / (2.0 * desiredDisplacement);
      return num / d * 0.001;
    }

    public static double GetExpectedDisplacement(this ManipulationInertiaStartingRoutedEventArgs e)
    {
      double d1 = e.TranslationBehavior.DesiredDisplacement;
      if (double.IsNaN(d1))
      {
        double d2 = e.TranslationBehavior.DesiredDeceleration;
        if (double.IsNaN(d2))
          d2 = 0.0036234234;
        double x = ((Point) e.Velocities.Linear).X;
        double y = ((Point) e.Velocities.Linear).Y;
        double num = Math.Sqrt(x * x + y * y);
        d1 = num * num / (2.0 * d2);
      }
      return d1;
    }

    public static double GetExpectedDisplacementX(this ManipulationInertiaStartingRoutedEventArgs e)
    {
      double x = ((Point) e.Velocities.Linear).X;
      if (x == 0.0)
        return 0.0;
      double y = ((Point) e.Velocities.Linear).Y;
      double num = Math.Sqrt(x * x + y * y);
      double d1 = e.TranslationBehavior.DesiredDisplacement;
      if (double.IsNaN(d1))
      {
        double d2 = e.TranslationBehavior.DesiredDeceleration;
        if (double.IsNaN(d2))
          d2 = 0.0036234234;
        d1 = num * num / (2.0 * d2);
      }
      return d1 * x / num;
    }

    public static double GetExpectedDisplacementY(this ManipulationInertiaStartingRoutedEventArgs e)
    {
      double y = ((Point) e.Velocities.Linear).Y;
      if (y == 0.0)
        return 0.0;
      double x = ((Point) e.Velocities.Linear).X;
      double num = Math.Sqrt(x * x + y * y);
      double d1 = e.TranslationBehavior.DesiredDisplacement;
      if (double.IsNaN(d1))
      {
        double d2 = e.TranslationBehavior.DesiredDeceleration;
        if (double.IsNaN(d2))
          d2 = 0.0036234234;
        d1 = num * num / (2.0 * d2);
      }
      return d1 * y / num;
    }
  }
}
