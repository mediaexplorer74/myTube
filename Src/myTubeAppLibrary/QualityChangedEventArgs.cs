// Decompiled with JetBrains decompiler
// Type: myTube.QualityChangedEventArgs
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;

namespace myTube
{
  public class QualityChangedEventArgs : EventArgs
  {
    public YouTubeQuality NewQuality;
    public YouTubeQuality OldQuality;
    public YouTubeQuality RequestedQuality;
    public bool ChangedByUser = true;
    public TimeSpan Position = TimeSpan.Zero;
  }
}
