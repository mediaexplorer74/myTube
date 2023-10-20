// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.JournalEntry
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;

namespace WinRTXamlToolkit.Controls
{
  public class JournalEntry
  {
    public Type SourcePageType { get; internal set; }

    public object Parameter { get; internal set; }

    public override bool Equals(object obj) => obj is JournalEntry journalEntry && this.SourcePageType.Equals(journalEntry.SourcePageType) && (this.Parameter == null && journalEntry.Parameter == null || this.Parameter.Equals(journalEntry.Parameter));

    public override int GetHashCode()
    {
      int num = 17;
      return (this.Parameter == null ? num * 23 : num * 23 + this.Parameter.GetHashCode()) * 23 + this.SourcePageType.GetHashCode();
    }
  }
}
