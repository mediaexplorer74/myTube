// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.ClientAndFirstLoadTask
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.Threading.Tasks;

namespace myTube.Helpers
{
  public class ClientAndFirstLoadTask
  {
    public VideoListClient Client;
    public Task<YouTubeEntry[]> LoadTask;
  }
}
