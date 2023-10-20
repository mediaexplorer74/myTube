// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistButtonsEventArgs
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;

namespace myTube
{
  public class PlaylistButtonsEventArgs : EventArgs
  {
    public bool PrevButton { get; internal set; }

    public bool NextButton { get; internal set; }
  }
}
