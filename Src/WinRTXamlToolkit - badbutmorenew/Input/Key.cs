// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.Key
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Input
{
  public static class Key
  {
    private static readonly Dictionary<string, VirtualKey> KeyMap = new Dictionary<string, VirtualKey>();

    static Key()
    {
      ((IEnumerable<VirtualKey>) ((IEnumerable<VirtualKey>) EnumExtensions.GetValues<VirtualKey>()).Where<VirtualKey>((Func<VirtualKey, bool>) (vk => !Key.KeyMap.ContainsKey(vk.ToString().ToLower())))).ForEach<VirtualKey>((Action<VirtualKey>) (vk => Key.KeyMap.Add(vk.ToString().ToLower(), vk)));
      Key.KeyMap.Add("alt", (VirtualKey) 18);
      Key.KeyMap.Add("ctrl", (VirtualKey) 17);
      Key.KeyMap.Add(" ", (VirtualKey) 32);
      Key.KeyMap.Add("+", (VirtualKey) 107);
      Key.KeyMap.Add("-", (VirtualKey) 109);
      Key.KeyMap.Add("/", (VirtualKey) 111);
      Key.KeyMap.Add("*", (VirtualKey) 106);
      Key.KeyMap.Add(".", (VirtualKey) 110);
      Key.KeyMap.Add("`", (VirtualKey) 192);
      Key.KeyMap.Add("~", (VirtualKey) 192);
      Key.KeyMap.Add(":", (VirtualKey) 186);
      Key.KeyMap.Add(";", (VirtualKey) 186);
      Key.KeyMap.Add(",", (VirtualKey) 188);
      Key.KeyMap.Add("?", (VirtualKey) 191);
      Key.KeyMap.Add("[", (VirtualKey) 219);
      Key.KeyMap.Add("{", (VirtualKey) 219);
      Key.KeyMap.Add("\\", (VirtualKey) 220);
      Key.KeyMap.Add("|", (VirtualKey) 220);
      Key.KeyMap.Add("]", (VirtualKey) 221);
      Key.KeyMap.Add("}", (VirtualKey) 221);
      Key.KeyMap.Add("'", (VirtualKey) 222);
      Key.KeyMap.Add("\"", (VirtualKey) 222);
    }

    public static VirtualKey Parse(string keyName)
    {
      VirtualKey virtualKey;
      if (!Key.KeyMap.TryGetValue(keyName.ToLower(), out virtualKey))
        throw new FormatException(string.Format("\"{0}\" is not a recognized key name. Check VirtualKey enumeration for known key names."));
      return virtualKey;
    }
  }
}
