// Decompiled with JetBrains decompiler
// Type: TestFramework.TestMethodInfo
// Assembly: TestFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D769B2DD-65AF-48DC-A0D8-37BA624BCB37
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\TestFramework.dll

using System;
using System.Reflection;

namespace TestFramework
{
  public class TestMethodInfo
  {
    public string ParameterSource;
    public object Parameter;

    public Type Type { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public MethodInfo MethodInfo { get; set; }

    public override string ToString() => this.Name;
  }
}
