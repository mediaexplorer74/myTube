// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.IBufferExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace WinRTXamlToolkit.Imaging
{
  public static class IBufferExtensions
  {
    public static IBufferExtensions.PixelBufferInfo GetPixels(this IBuffer pixelBuffer) => new IBufferExtensions.PixelBufferInfo(pixelBuffer);

    public class PixelBufferInfo
    {
      private readonly Stream _pixelStream;
      public byte[] Bytes;

      public int this[int i]
      {
        get => ColorExtensions.IntColorFromBytes(this.Bytes[i * 4 + 3], this.Bytes[i * 4 + 2], this.Bytes[i * 4 + 1], this.Bytes[i * 4]);
        set
        {
          this.Bytes[i * 4 + 3] = (byte) (value >> 24 & (int) byte.MaxValue);
          this.Bytes[i * 4 + 2] = (byte) (value >> 16 & (int) byte.MaxValue);
          this.Bytes[i * 4 + 1] = (byte) (value >> 8 & (int) byte.MaxValue);
          this.Bytes[i * 4] = (byte) (value & (int) byte.MaxValue);
          this._pixelStream.Seek((long) (i * 4), SeekOrigin.Begin);
          this._pixelStream.Write(this.Bytes, i * 4, 4);
        }
      }

      public byte MaxDiff(int i, int color) => Math.Max(Math.Max(Math.Max((byte) Math.Abs((int) this.Bytes[i * 4 + 3] - (color >> 24 & (int) byte.MaxValue)), (byte) Math.Abs((int) this.Bytes[i * 4 + 2] - (color >> 16 & (int) byte.MaxValue))), (byte) Math.Abs((int) this.Bytes[i * 4 + 1] - (color >> 8 & (int) byte.MaxValue))), (byte) Math.Abs((int) this.Bytes[i * 4] - (color & (int) byte.MaxValue)));

      public PixelBufferInfo(IBuffer pixelBuffer)
      {
        this._pixelStream = WindowsRuntimeBufferExtensions.AsStream(pixelBuffer);
        this.Bytes = new byte[this._pixelStream.Length];
        this._pixelStream.Seek(0L, SeekOrigin.Begin);
        this._pixelStream.Read(this.Bytes, 0, this.Bytes.Length);
      }

      public void UpdateFromBytes()
      {
        this._pixelStream.Seek(0L, SeekOrigin.Begin);
        this._pixelStream.Write(this.Bytes, 0, this.Bytes.Length);
      }
    }
  }
}
