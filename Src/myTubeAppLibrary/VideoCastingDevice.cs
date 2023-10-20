// Decompiled with JetBrains decompiler
// Type: myTube.VideoCastingDevice
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;

namespace myTube
{
  public class VideoCastingDevice
  {
    public string Name { get; set; } = "Device";

    public string Id { get; set; }

    internal DateTimeOffset AddedAt { get; set; } = DateTimeOffset.MinValue;

    public CastingDeviceType DeviceType { get; set; }

    public IVideoHandler VideoHandler { get; internal set; }

    public object UnderlyingCastingObject { get; private set; }

    public VideoCastingDevice(
      object underlyingCastingOBject,
      string name,
      string id,
      IVideoHandler videoHandler)
    {
      this.UnderlyingCastingObject = underlyingCastingOBject;
      this.Id = id;
      this.Name = name;
      this.VideoHandler = videoHandler;
    }

    public void Update(VideoCastingDevice newDevice)
    {
      this.UnderlyingCastingObject = newDevice.UnderlyingCastingObject;
      this.Id = newDevice.Id;
      this.Name = newDevice.Name;
      this.VideoHandler = newDevice.VideoHandler;
    }
  }
}
