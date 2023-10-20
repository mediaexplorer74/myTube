// Decompiled with JetBrains decompiler
// Type: myTube.VideoHandlerFallbackRequestedException
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;

namespace myTube
{
  public class VideoHandlerFallbackRequestedException : Exception
  {
    public VideoHandlerFallbackRequestedException()
      : base("This video handler cannot play the following video, fallback is requested")
    {
    }

    public VideoHandlerFallbackRequestedException(string message)
      : base(message)
    {
    }

    public VideoHandlerFallbackRequestedException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
