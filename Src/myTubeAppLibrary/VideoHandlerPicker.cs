// Decompiled with JetBrains decompiler
// Type: myTube.VideoHandlerPicker
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myTube
{
  public class VideoHandlerPicker
  {
    private List<VideoHandlerModel> models = new List<VideoHandlerModel>();
    private VideoHandlerModel current;

    public void AddHandlerModel(VideoHandlerModel model) => this.models.Add(model);

    public IVideoHandler PickVideoHandler(YouTubeEntry entry, YouTubeQuality quality)
    {
      if (this.current != null && this.current.ShouldKeepUsing && (!this.current.VideoHandler.CastingIsRequired || this.current.VideoHandler.VideoCastingDevice != null) && (entry.LiveStatus == LiveStatus.None || this.current.CanPlayLiveVideo))
        return this.current.VideoHandler;
      if (entry.LiveStatus != LiveStatus.None)
      {
        VideoHandlerModel videoHandlerModel = this.models.Where<VideoHandlerModel>((Func<VideoHandlerModel, bool>) (m => m.CanPlayLiveVideo)).FirstOrDefault<VideoHandlerModel>();
        if (videoHandlerModel != null)
          return videoHandlerModel.VideoHandler;
      }
      if (quality == YouTubeQuality.Audio)
      {
        VideoHandlerModel videoHandlerModel = this.models.Where<VideoHandlerModel>((Func<VideoHandlerModel, bool>) (m => m.CanPlayBackgroundAudio)).FirstOrDefault<VideoHandlerModel>();
        if (videoHandlerModel != null)
          return videoHandlerModel.VideoHandler;
      }
      return this.models.FirstOrDefault<VideoHandlerModel>().VideoHandler;
    }

    public IVideoHandler PickCastingHandler(VideoCastingDevice device) => (this.models.Where<VideoHandlerModel>((Func<VideoHandlerModel, bool>) (m => m.VideoHandler == device.VideoHandler)).FirstOrDefault<VideoHandlerModel>() ?? throw new InvalidOperationException("The video handler attached to this casting device does not exist in this object")).VideoHandler;
  }
}
