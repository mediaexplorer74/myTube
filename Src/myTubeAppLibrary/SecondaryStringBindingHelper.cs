// Decompiled with JetBrains decompiler
// Type: myTube.SecondaryStringBindingHelper
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

namespace myTube
{
  public struct SecondaryStringBindingHelper
  {
    private Strings strings;
    private string path;

    public string this[string fallback] => this.strings == null ? "" : this.strings[this.path, fallback];

    public SecondaryStringBindingHelper(Strings strings, string path)
    {
      this.strings = strings;
      this.path = path;
    }
  }
}
