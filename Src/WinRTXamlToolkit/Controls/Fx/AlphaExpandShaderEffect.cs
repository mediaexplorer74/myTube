// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Fx.AlphaExpandShaderEffect
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
  public class AlphaExpandShaderEffect : CpuShaderEffect
  {
    public Color Color { get; set; }

    public AlphaExpandShaderEffect() => this.Color = (Color) Colors.Red;

    public override async Task ProcessBitmap(
      RenderTargetBitmap rtb,
      WriteableBitmap wb,
      int pw,
      int ph)
    {
      IBuffer rtbBuffer = await rtb.GetPixelsAsync();
      IBufferExtensions.PixelBufferInfo rtbPixels = rtbBuffer.GetPixels();
      IBuffer wbBuffer = wb.PixelBuffer;
      IBufferExtensions.PixelBufferInfo wbPixels = wbBuffer.GetPixels();
      byte r = this.Color.R;
      byte g = this.Color.G;
      byte b = this.Color.B;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        for (int index2 = 0; index2 < ph; ++index2)
        {
          int num1 = Math.Max(0, index1 - 1);
          int num2 = Math.Min(index1 + 1, pw - 1);
          int num3 = Math.Max(0, index2 - 1);
          int num4 = Math.Min(index2 + 1, ph - 1);
          byte num5 = 0;
          for (int index3 = num1; index3 <= num2; ++index3)
          {
            for (int index4 = num3; index4 <= num4; ++index4)
            {
              byte num6 = rtbPixels.Bytes[4 * (index4 * pw + index3) + 3];
              if ((int) num6 > (int) num5)
                num5 = num6;
            }
          }
          wbPixels.Bytes[4 * (index2 * pw + index1)] = b;
          wbPixels.Bytes[4 * (index2 * pw + index1) + 1] = g;
          wbPixels.Bytes[4 * (index2 * pw + index1) + 2] = r;
          wbPixels.Bytes[4 * (index2 * pw + index1) + 3] = num5;
        }
      }
      wbPixels.UpdateFromBytes();
      wb.Invalidate();
    }
  }
}
