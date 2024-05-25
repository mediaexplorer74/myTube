// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Fx.BlurShaderEffect
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
  public class BlurShaderEffect : CpuShaderEffect
  {
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
      int radius = 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        for (int index2 = 0; index2 < ph; ++index2)
        {
          int num1 = Math.Max(0, index1 - radius);
          int num2 = Math.Min(index1 + radius, pw - 1);
          int num3 = Math.Max(0, index2 - radius);
          int num4 = Math.Min(index2 + radius, ph - 1);
          int num5 = (num2 - num1 + 1) * (num4 - num3 + 1) + 7;
          int[] numArray = new int[4];
          for (int index3 = num1; index3 <= num2; ++index3)
          {
            for (int index4 = num3; index4 <= num4; ++index4)
            {
              for (int index5 = 0; index5 < 4; ++index5)
                numArray[index5] += index1 != index3 || index2 != index4 ? (int) rtbPixels.Bytes[4 * (index4 * pw + index3) + index5] : (int) rtbPixels.Bytes[4 * (index4 * pw + index3) + index5] * 8;
            }
          }
          for (int index6 = 0; index6 < 4; ++index6)
            wbPixels.Bytes[4 * (index2 * pw + index1) + index6] = (byte) (numArray[index6] / num5);
        }
      }
      wbPixels.UpdateFromBytes();
      wb.Invalidate();
    }
  }
}
