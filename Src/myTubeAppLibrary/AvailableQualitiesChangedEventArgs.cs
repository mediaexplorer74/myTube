// Decompiled with JetBrains decompiler
// Type: myTube.AvailableQualitiesChangedEventArgs
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;

namespace myTube
{
  public class AvailableQualitiesChangedEventArgs : EventArgs
  {
    public IEnumerable<YouTubeQuality> AvailableQualities;
  }
}
