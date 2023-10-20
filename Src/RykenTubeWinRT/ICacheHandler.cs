// Decompiled with JetBrains decompiler
// Type: RykenTube.ICacheHandler
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Threading.Tasks;

namespace RykenTube
{
  public interface ICacheHandler
  {
    Task<CacheInfo> LoadCache(string group, string name);

    Task<bool> SaveCache(CacheInfo cache);

    Task RemoveCache(string group, string name);

    Task RemoveGroup(string group);

    Task EstablishGroup(string group, GroupCacheInfo info);
  }
}
