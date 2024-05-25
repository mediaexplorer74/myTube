// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AlternativeNavigatingCancelEventArgs
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
  public class AlternativeNavigatingCancelEventArgs
  {
    public bool Cancel { get; set; }

    public NavigationMode NavigationMode { get; private set; }

    public Type SourcePageType { get; private set; }

    public AlternativeNavigatingCancelEventArgs(NavigationMode navigationMode, Type sourcePageType)
    {
      this.NavigationMode = navigationMode;
      this.SourcePageType = sourcePageType;
    }
  }
}
