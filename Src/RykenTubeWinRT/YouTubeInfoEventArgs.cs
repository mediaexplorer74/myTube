// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeInfoEventArgs
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class YouTubeInfoEventArgs : EventArgs
  {
    public YouTubeInfo Info;
    public bool HD;
    public bool SD;
    public bool LQ;
    public string Result = "";
    public int Tries;
    public bool ForPlayback = true;
    public bool Deciphered;
    public bool WatchPage;
  }
}
