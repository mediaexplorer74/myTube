// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.VirtualKeyExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.System;

namespace WinRTXamlToolkit.Input
{
  public static class VirtualKeyExtensions
  {
    public static bool IsModifier(this VirtualKey key) => key == 17 || key == 18 || key == 16 || key == 162 || key == 164 || key == 160 || key == 163 || key == 165 || key == 161 || key == 91 || key == 92 || key == 93;
  }
}
