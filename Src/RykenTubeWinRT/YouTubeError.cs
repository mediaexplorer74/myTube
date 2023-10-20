// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeError
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;

namespace RykenTube
{
  public class YouTubeError
  {
    public string Method;
    public string Action;
    public string Reason;
    public Dictionary<string, object> OtherInfo = new Dictionary<string, object>();
    public Exception Exception;
  }
}
