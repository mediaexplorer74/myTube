// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.EnumExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
  public static class EnumExtensions
  {
    public static TEnumType[] GetValues<TEnumType>(Func<TEnumType, bool> condition = null) => condition == null ? ((IEnumerable) Enum.GetValues(typeof (TEnumType))).Cast<TEnumType>().ToArray<TEnumType>() : ((IEnumerable) Enum.GetValues(typeof (TEnumType))).Cast<TEnumType>().Where<TEnumType>((Func<TEnumType, bool>) condition).ToArray<TEnumType>();
  }
}
