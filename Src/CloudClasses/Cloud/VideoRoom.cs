// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.VideoRoom
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using System;

namespace myTube.Cloud
{
  public class VideoRoom : DataObject
  {
    public static char[] AcceptablePasswordCharacters = new char[8]
    {
      'a',
      'b',
      'c',
      'd',
      '1',
      '2',
      '3',
      '4'
    };

    public string HostName { get; set; }

    public string Description { get; set; }

    public string Password { get; set; }

    public string Title { get; set; }

    public string ChannelId { get; set; }

    public string ChannelName { get; set; }

    public int ViewerCount { get; set; }

    public DateTime LastPingAt { get; set; }

    public RoomPrivacy Privacy { get; set; }

    public double Score() => 10.0 + (double) (this.ViewerCount * 7) - Math.Max(1.0, (DateTime.Now - this.LastPingAt).TotalMinutes) * 5.0;
  }
}
