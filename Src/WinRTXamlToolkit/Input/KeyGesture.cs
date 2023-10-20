// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.KeyGesture
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Input
{
  public class KeyGesture : List<KeyCombination>
  {
    public override string ToString() => string.Join(",", (IEnumerable<string>) this.Select<KeyCombination, string>((Func<KeyCombination, string>) (c => ((object) c).ToString())));

    public static KeyGesture Parse(string keyGestureString)
    {
      keyGestureString = keyGestureString != null ? keyGestureString.Trim() : throw new ArgumentNullException(nameof (keyGestureString), "Key gesture string not specified");
      if (keyGestureString.Length == 0)
        throw new FormatException("Key gesture string empty");
      KeyGesture keyGesture = new KeyGesture();
      try
      {
        KeyCombination keyCombination = (KeyCombination) null;
        int startIndex = 0;
        for (int index = 0; index < keyGestureString.Length; ++index)
        {
          switch (keyGestureString[index])
          {
            case '+':
              if (keyCombination == null)
                keyCombination = new KeyCombination();
              keyCombination.Add(Key.Parse(keyGestureString.Substring(startIndex, index - startIndex)));
              startIndex = index + 1;
              break;
            case ',':
              if (keyCombination == null)
                keyCombination = new KeyCombination();
              keyCombination.Add(Key.Parse(keyGestureString.Substring(startIndex, index - startIndex)));
              startIndex = index + 1;
              keyGesture.Add(keyCombination);
              keyCombination = (KeyCombination) null;
              break;
          }
        }
        if (startIndex < keyGestureString.Length)
        {
          if (keyCombination == null)
            keyCombination = new KeyCombination();
          keyCombination.Add(Key.Parse(keyGestureString.Substring(startIndex)));
          keyGesture.Add(keyCombination);
        }
      }
      catch (Exception ex)
      {
        throw new FormatException(string.Format("Key gesture string \"{0}\" not recognized", (object) keyGestureString), ex);
      }
      return keyGesture;
    }
  }
}
