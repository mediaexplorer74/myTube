// Decompiled with JetBrains decompiler
// Type: RykenTube.SIDXInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;

namespace RykenTube
{
  public class SIDXInfo
  {
    public ushort Reserved;
    public ushort ReferenceCount;
    public uint ReferenceID;
    public uint TimeScale;
    public ulong EarliestPresentationTime;
    public ulong FirstOffset;
    public List<SIDXSegment> Segments = new List<SIDXSegment>();

    public List<OffsetAndTimeStamp> Offsets()
    {
      TimeSpan zero = TimeSpan.Zero;
      long num = 0;
      List<OffsetAndTimeStamp> offsetAndTimeStampList = new List<OffsetAndTimeStamp>();
      foreach (SIDXSegment segment in this.Segments)
      {
        offsetAndTimeStampList.Add(new OffsetAndTimeStamp()
        {
          Segment = segment,
          ByteOffset = num,
          TimeStamp = zero
        });
        num += (long) segment.ReferencedSize;
        zero += TimeSpan.FromSeconds((double) segment.SubsegmentDuration / (double) this.TimeScale);
      }
      return offsetAndTimeStampList;
    }
  }
}
