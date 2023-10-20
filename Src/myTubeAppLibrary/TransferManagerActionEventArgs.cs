// Decompiled with JetBrains decompiler
// Type: myTube.TransferManagerActionEventArgs
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

namespace myTube
{
  public class TransferManagerActionEventArgs
  {
    public TransferManagerAction Action;
    public TransferType Type = TransferType.Video;
    public string VideoID;
  }
}
