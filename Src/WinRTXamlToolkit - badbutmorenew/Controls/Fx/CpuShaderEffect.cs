// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Fx.CpuShaderEffect
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
  public abstract class CpuShaderEffect
  {
    public abstract Task ProcessBitmap(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph);
  }
}
