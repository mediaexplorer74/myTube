// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AlternativeNavigationEventArgs
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
  public class AlternativeNavigationEventArgs : EventArgs
  {
    public object Content { get; private set; }

    public NavigationMode NavigationMode { get; private set; }

    public object Parameter { get; private set; }

    public Type SourcePageType { get; private set; }

    public AlternativeNavigationEventArgs(
      object content,
      NavigationMode navigationMode,
      object parameter,
      Type sourcePageType)
    {
      this.Content = content;
      this.NavigationMode = navigationMode;
      this.Parameter = parameter;
      this.SourcePageType = sourcePageType;
    }
  }
}
