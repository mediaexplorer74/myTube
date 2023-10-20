// Decompiled with JetBrains decompiler
// Type: myTube.VideoInfoPreload
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Threading.Tasks;

namespace myTube
{
  public class VideoInfoPreload
  {
    public Task DataTask;

    internal VideoInfoPreload()
    {
    }

    public DateTimeOffset CreatedAt { get; internal set; }

    public DateTimeOffset ExpiresAt { get; internal set; }

    public bool IsExpired => DateTimeOffset.Now > this.ExpiresAt;

    public async Task<T> GetData<T>() => this.DataTask is Task<T> dataTask ? await dataTask : default (T);
  }
}
