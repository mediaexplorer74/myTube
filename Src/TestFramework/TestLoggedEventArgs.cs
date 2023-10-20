// Decompiled with JetBrains decompiler
// Type: TestFramework.TestLoggedEventArgs
// Assembly: TestFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D769B2DD-65AF-48DC-A0D8-37BA624BCB37
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\TestFramework.dll

using System;

namespace TestFramework
{
  public class TestLoggedEventArgs : EventArgs
  {
    public string Text { get; set; }

    public LogType Type { get; set; }
  }
}
