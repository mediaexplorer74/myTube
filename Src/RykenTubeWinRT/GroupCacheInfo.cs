// Decompiled with JetBrains decompiler
// Type: RykenTube.GroupCacheInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class GroupCacheInfo
  {
    public int MaxItems { get; set; }

    public TimeSpan MaxAge { get; set; }

    public GroupCacheInfo()
    {
      this.MaxItems = 100;
      this.MaxAge = TimeSpan.FromHours(5.0);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) (obj as GroupCacheInfo) == null)
        return base.Equals(obj);
      GroupCacheInfo groupCacheInfo = obj as GroupCacheInfo;
      return groupCacheInfo.MaxItems == this.MaxItems && groupCacheInfo.MaxAge == this.MaxAge;
    }

    public static bool operator ==(GroupCacheInfo g1, GroupCacheInfo g2) => (object) g1 == null ? (object) g2 == null : g1.Equals((object) g2);

    public static bool operator !=(GroupCacheInfo g1, GroupCacheInfo g2) => (object) g1 == null ? g2 != null : !g1.Equals((object) g2);
  }
}
