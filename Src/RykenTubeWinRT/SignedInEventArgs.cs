// Decompiled with JetBrains decompiler
// Type: RykenTube.SignedInEventArgs
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class SignedInEventArgs : EventArgs
  {
    public string PreviousAccessToken { get; set; }

    public string PreviousRefreshToken { get; set; }

    public string NewRefreshToken { get; set; }

    public string NewAccessToken { get; set; }

    public bool IsRefreshSignIn { get; set; }
  }
}
