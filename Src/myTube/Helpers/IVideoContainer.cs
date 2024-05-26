// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.IVideoContainer
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using Windows.UI.Xaml;

namespace myTube.Helpers
{
  public interface IVideoContainer
  {
    void VideoSet();

    void VideoUnset();

    bool GetBindVideoPlayerShown();

    VideoDepth GetVideoDepth();

    FrameworkElement GetElement();

    bool HasBackground();

    bool IsArrangeActive();
  }
}
