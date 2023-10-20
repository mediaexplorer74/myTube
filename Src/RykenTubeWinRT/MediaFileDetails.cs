// Decompiled with JetBrains decompiler
// Type: RykenTube.MediaFileDetails
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace RykenTube
{
  public class MediaFileDetails
  {
    private uint TimeScale = 90000;
    private uint ScaledDuration;

    public TimeSpan Duration => TimeSpan.FromSeconds((double) this.ScaledDuration / (double) this.TimeScale);

    public double FramesPerSecond { get; private set; } = 30.0;

    public TimeSpan FrameTime => TimeSpan.FromSeconds(1.0 / this.FramesPerSecond);

    public static async Task<MediaFileDetails> GetFromYouTubeInfo(URLConstructor con)
    {
      HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
      ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Range", "bytes=" + (object) 0 + "-" + (object) 24576);
      MediaBoxReader mediaBoxReader = new MediaBoxReader((await (await httpClient.GetAsync(con.ToUri())).Content.ReadAsBufferAsync()).ToArray());
      MediaFileDetails fromYouTubeInfo = new MediaFileDetails();
      if (mediaBoxReader.SeekToBox("mvhd"))
      {
        int num1 = (int) mediaBoxReader.ReadUInt32();
        mediaBoxReader.ReadString((byte) 32);
        int num2 = (int) mediaBoxReader.ReadByte();
        int num3 = (int) mediaBoxReader.ReadUInt32((byte) 24);
        int num4 = (int) mediaBoxReader.ReadUInt32();
        int num5 = (int) mediaBoxReader.ReadUInt32();
        fromYouTubeInfo.TimeScale = mediaBoxReader.ReadUInt32();
        fromYouTubeInfo.ScaledDuration = mediaBoxReader.ReadUInt32();
      }
      if (mediaBoxReader.SeekToBox("stts"))
      {
        int num6 = (int) mediaBoxReader.ReadUInt32();
        mediaBoxReader.ReadString((byte) 32);
        int num7 = (int) mediaBoxReader.ReadByte();
        int num8 = (int) mediaBoxReader.ReadUInt32((byte) 24);
        uint num9 = mediaBoxReader.ReadUInt32();
        if (num9 > 0U)
        {
          ulong num10 = 0;
          int num11 = 0;
          for (int index = 0; (long) index < (long) num9 && mediaBoxReader.RemainingBits >= 64; ++index)
          {
            int num12 = (int) mediaBoxReader.ReadUInt32();
            uint num13 = mediaBoxReader.ReadUInt32();
            num10 += (ulong) num13;
            ++num11;
          }
          double num14 = (double) num10 / (double) num11;
          fromYouTubeInfo.FramesPerSecond = (double) fromYouTubeInfo.TimeScale / num14;
        }
      }
      return fromYouTubeInfo;
    }
  }
}
