// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.AnimationHelper
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class AnimationHelper
  {
    public static readonly DependencyProperty StoryboardProperty = DependencyProperty.RegisterAttached("Storyboard", (Type) typeof (Storyboard), (Type) typeof (AnimationHelper), new PropertyMetadata((object) null, new PropertyChangedCallback(AnimationHelper.OnStoryboardChanged)));
    public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.RegisterAttached("IsPlaying", (Type) typeof (bool), (Type) typeof (AnimationHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(AnimationHelper.OnIsPlayingChanged)));

    public static Storyboard GetStoryboard(DependencyObject d) => (Storyboard) d.GetValue(AnimationHelper.StoryboardProperty);

    public static void SetStoryboard(DependencyObject d, Storyboard value) => d.SetValue(AnimationHelper.StoryboardProperty, (object) value);

    private static void OnStoryboardChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Storyboard oldValue = (Storyboard) e.OldValue;
      Storyboard storyboard = (Storyboard) d.GetValue(AnimationHelper.StoryboardProperty);
      oldValue?.Stop();
      if (!AnimationHelper.GetIsPlaying(d) || storyboard == null)
        return;
      storyboard.Begin();
    }

    public static bool GetIsPlaying(DependencyObject d) => (bool) d.GetValue(AnimationHelper.IsPlayingProperty);

    public static void SetIsPlaying(DependencyObject d, bool value) => d.SetValue(AnimationHelper.IsPlayingProperty, (object) value);

    private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      int num = (bool) e.OldValue ? 1 : 0;
      bool flag = (bool) d.GetValue(AnimationHelper.IsPlayingProperty);
      Storyboard storyboard = AnimationHelper.GetStoryboard(d);
      if (storyboard == null)
        return;
      if (!flag)
        storyboard.Stop();
      if (!flag)
        return;
      storyboard.Begin();
    }
  }
}
