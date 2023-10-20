// Decompiled with JetBrains decompiler
// Type: myTube.MediaThumbnailChanged
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public class MediaThumbnailChanged : EventArgs
  {
    public Stretch StretchMode { get; set; } = (Stretch) 3;

    public Uri Uri { get; set; }

    public IRandomAccessStream Stream { get; set; }
  }
}
