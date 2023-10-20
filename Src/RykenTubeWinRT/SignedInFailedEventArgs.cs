// Decompiled with JetBrains decompiler
// Type: RykenTube.SignedInFailedEventArgs
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class SignedInFailedEventArgs : EventArgs
  {
    public bool Critical;
    public string Reason = "Unspecified";
  }
}
