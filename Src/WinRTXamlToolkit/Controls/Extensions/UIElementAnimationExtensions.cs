// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.UIElementAnimationExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class UIElementAnimationExtensions
  {
    public static readonly DependencyProperty AttachedFadeStoryboardProperty = DependencyProperty.RegisterAttached("AttachedFadeStoryboard", (Type) typeof (Storyboard), (Type) typeof (UIElementAnimationExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(UIElementAnimationExtensions.OnAttachedFadeStoryboardChanged)));

    private static Storyboard GetAttachedFadeStoryboard(DependencyObject d) => (Storyboard) d.GetValue(UIElementAnimationExtensions.AttachedFadeStoryboardProperty);

    private static void SetAttachedFadeStoryboard(DependencyObject d, Storyboard value) => d.SetValue(UIElementAnimationExtensions.AttachedFadeStoryboardProperty, (object) value);

    private static void OnAttachedFadeStoryboardChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Storyboard oldValue = (Storyboard) e.OldValue;
      Storyboard storyboard = (Storyboard) d.GetValue(UIElementAnimationExtensions.AttachedFadeStoryboardProperty);
    }

    public static async Task FadeIn(this UIElement element, TimeSpan? duration = null)
    {
      element.put_Visibility((Visibility) 0);
      Storyboard fadeInStoryboard = new Storyboard();
      FadeInThemeAnimation fadeInAnimation = new FadeInThemeAnimation();
      if (duration.HasValue)
        ((Timeline) fadeInAnimation).put_Duration((Duration) (TimeSpan) duration.Value);
      Storyboard.SetTarget((Timeline) fadeInAnimation, (DependencyObject) element);
      ((ICollection<Timeline>) fadeInStoryboard.Children).Add((Timeline) fadeInAnimation);
      await fadeInStoryboard.BeginAsync();
    }

    public static async Task FadeOut(this UIElement element, TimeSpan? duration = null)
    {
      Storyboard fadeOutStoryboard = new Storyboard();
      FadeOutThemeAnimation fadeOutAnimation = new FadeOutThemeAnimation();
      if (duration.HasValue)
        ((Timeline) fadeOutAnimation).put_Duration((Duration) (TimeSpan) duration.Value);
      Storyboard.SetTarget((Timeline) fadeOutAnimation, (DependencyObject) element);
      ((ICollection<Timeline>) fadeOutStoryboard.Children).Add((Timeline) fadeOutAnimation);
      await fadeOutStoryboard.BeginAsync();
    }

    public static async Task FadeInCustom(
      this UIElement element,
      TimeSpan? duration = null,
      EasingFunctionBase easingFunction = null,
      double targetOpacity = 1.0)
    {
      element.CleanUpPreviousFadeStoryboard();
      Storyboard fadeInStoryboard = new Storyboard();
      DoubleAnimation fadeInAnimation = new DoubleAnimation();
      if (!duration.HasValue)
        duration = new TimeSpan?(TimeSpan.FromSeconds(0.4));
      ((Timeline) fadeInAnimation).put_Duration((Duration) (TimeSpan) duration.Value);
      fadeInAnimation.put_To((double?) new double?(targetOpacity));
      fadeInAnimation.put_EasingFunction(easingFunction);
      Storyboard.SetTarget((Timeline) fadeInAnimation, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) fadeInAnimation, "Opacity");
      ((ICollection<Timeline>) fadeInStoryboard.Children).Add((Timeline) fadeInAnimation);
      UIElementAnimationExtensions.SetAttachedFadeStoryboard((DependencyObject) element, fadeInStoryboard);
      await fadeInStoryboard.BeginAsync();
      element.put_Opacity(targetOpacity);
      fadeInStoryboard.Stop();
    }

    public static async Task FadeOutCustom(
      this UIElement element,
      TimeSpan? duration = null,
      EasingFunctionBase easingFunction = null)
    {
      element.CleanUpPreviousFadeStoryboard();
      Storyboard fadeOutStoryboard = new Storyboard();
      DoubleAnimation fadeOutAnimation = new DoubleAnimation();
      if (!duration.HasValue)
        duration = new TimeSpan?(TimeSpan.FromSeconds(0.4));
      ((Timeline) fadeOutAnimation).put_Duration((Duration) (TimeSpan) duration.Value);
      fadeOutAnimation.put_To((double?) new double?(0.0));
      fadeOutAnimation.put_EasingFunction(easingFunction);
      Storyboard.SetTarget((Timeline) fadeOutAnimation, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) fadeOutAnimation, "Opacity");
      ((ICollection<Timeline>) fadeOutStoryboard.Children).Add((Timeline) fadeOutAnimation);
      UIElementAnimationExtensions.SetAttachedFadeStoryboard((DependencyObject) element, fadeOutStoryboard);
      await fadeOutStoryboard.BeginAsync();
      element.put_Opacity(0.0);
      fadeOutStoryboard.Stop();
    }

    public static void CleanUpPreviousFadeStoryboard(this UIElement element) => UIElementAnimationExtensions.GetAttachedFadeStoryboard((DependencyObject) element)?.Stop();
  }
}
