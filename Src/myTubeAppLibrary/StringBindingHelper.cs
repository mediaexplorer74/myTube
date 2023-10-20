// Decompiled with JetBrains decompiler
// Type: myTube.StringBindingHelper
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

namespace myTube
{
  public class StringBindingHelper
  {
    private SecondaryStringBindingHelper helper;

    public Strings Source { get; set; }

    public SecondaryStringBindingHelper this[string path] => new SecondaryStringBindingHelper(this.Source, path);
  }
}
