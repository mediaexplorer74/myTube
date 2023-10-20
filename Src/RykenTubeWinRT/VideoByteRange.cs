// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoByteRange
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public struct VideoByteRange
  {
    public ulong Start;
    public ulong End;

    public long Length => (long) this.End - (long) this.Start + 1L;

    public static bool TryParse(string value, out VideoByteRange range)
    {
      range = new VideoByteRange();
      if (value.Contains("-"))
      {
        string[] strArray = value.Split(new string[1]{ "-" }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length == 2 && ulong.TryParse(strArray[0], out range.Start) && ulong.TryParse(strArray[1], out range.End))
          return true;
      }
      return false;
    }

    public override string ToString() => string.Format("{0}-{1}", (object) this.Start, (object) this.End);
  }
}
