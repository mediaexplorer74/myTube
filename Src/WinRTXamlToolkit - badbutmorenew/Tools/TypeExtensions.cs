// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.TypeExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Reflection;

namespace WinRTXamlToolkit.Tools
{
  public static class TypeExtensions
  {
    public static bool IsNullable(this Type type) => ((Type) type).GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
  }
}
