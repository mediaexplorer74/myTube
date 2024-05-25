// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.DissolveTransition
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

namespace WinRTXamlToolkit.Controls
{
  public class DissolveTransition : PageTransition
  {
    protected override PageTransitionMode Mode => PageTransitionMode.Parallel;

    public DissolveTransition()
    {
      this.ForwardOutAnimation = (PageTransitionAnimation) null;
      FadeAnimation fadeAnimation1 = new FadeAnimation();
      fadeAnimation1.Mode = AnimationMode.In;
      this.ForwardInAnimation = (PageTransitionAnimation) fadeAnimation1;
      this.BackwardOutAnimation = (PageTransitionAnimation) null;
      FadeAnimation fadeAnimation2 = new FadeAnimation();
      fadeAnimation2.Mode = AnimationMode.In;
      this.BackwardInAnimation = (PageTransitionAnimation) fadeAnimation2;
    }
  }
}
