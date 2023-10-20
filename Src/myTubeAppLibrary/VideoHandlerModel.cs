// Decompiled with JetBrains decompiler
// Type: myTube.VideoHandlerModel
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

namespace myTube
{
  public class VideoHandlerModel
  {
    public IVideoHandler VideoHandler { get; internal set; }

    public bool CanPlayLiveVideo { get; set; }

    public bool CanCast { get; set; }

    public bool CanPlayBackgroundAudio { get; set; }

    public bool ShouldKeepUsing { get; set; } = true;

    internal bool Default { get; set; }

    public VideoHandlerModel(IVideoHandler handler) => this.VideoHandler = handler;
  }
}
