// Decompiled with JetBrains decompiler
// Type: TestFramework.TestFailureException
// Assembly: TestFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D769B2DD-65AF-48DC-A0D8-37BA624BCB37
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\TestFramework.dll

using System;

namespace TestFramework
{
  public class TestFailureException : Exception
  {
    public TestFailureException(string message)
      : base(message)
    {
    }

    public TestFailureException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
