// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FadeAnimation
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
  public class FadeAnimation : PageTransitionAnimation
  {
    protected override Storyboard Animation
    {
      get
      {
        Storyboard animation = new Storyboard();
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        Storyboard.SetTargetProperty((Timeline) doubleAnimation, "Opacity");
        doubleAnimation.put_EasingFunction(this.EasingFunction);
        ((Timeline) doubleAnimation).put_Duration(this.Duration);
        doubleAnimation.put_From((double?) new double?(this.Mode == AnimationMode.In ? 0.0 : 1.0));
        doubleAnimation.put_To((double?) new double?(this.Mode == AnimationMode.In ? 1.0 : 0.0));
        ((ICollection<Timeline>) animation.Children).Add((Timeline) doubleAnimation);
        return animation;
      }
    }
  }
}
