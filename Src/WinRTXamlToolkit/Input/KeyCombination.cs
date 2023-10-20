// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.KeyCombination
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;

namespace WinRTXamlToolkit.Input
{
  public class KeyCombination : List<VirtualKey>
  {
    public override string ToString() => string.Join("+", (IEnumerable<string>) this.Select<VirtualKey, string>((Func<VirtualKey, string>) (vk => vk.ToString())));
  }
}
