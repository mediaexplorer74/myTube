// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ControlExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ControlExtensions
  {
    public static void MoveFocusForward(this Control control)
    {
      List<Control> list = ((IEnumerable<Control>) ((DependencyObject) Window.Current.Content).GetDescendantsOfType<Control>()).Where<Control>((Func<Control, bool>) (d => d.IsTabStop)).ToList<Control>();
      if (list.Count == 0)
        return;
      int num = list.IndexOf(control);
      if (num < 0)
        list[0].Focus((FocusState) 3);
      else if (num + 1 < list.Count)
        list[num + 1].Focus((FocusState) 3);
      else
        list[0].Focus((FocusState) 3);
    }
  }
}
