// Decompiled with JetBrains decompiler
// Type: myTube.TileArgs
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;

namespace myTube
{
  public class TileArgs
  {
    public int OverCanvasPage = -1;
    public string PageType;
    public string Param;
    public bool Play;
    public TypeConstructor Constructor;
    public bool ShouldSignInFirst = true;

    public TileArgs()
    {
    }

    public TileArgs(Type pageType, string param = null, int overCanvasPage = -1)
    {
      this.PageType = pageType.ToString();
      this.Param = param;
      this.OverCanvasPage = overCanvasPage;
    }

    public TileArgs(string pageType, string param = null, int overCanvasPage = -1)
    {
      this.PageType = pageType;
      this.Param = param;
      this.OverCanvasPage = overCanvasPage;
    }

    public TileArgs(URLConstructor url)
    {
      if (url.ContainsKey(nameof (OverCanvasPage)))
        int.TryParse(url[nameof (OverCanvasPage)], out this.OverCanvasPage);
      if (url.ContainsKey(nameof (PageType)))
        this.PageType = url[nameof (PageType)];
      if (url.ContainsKey(nameof (Param)))
        this.Param = url[nameof (Param)];
      if (url.ContainsKey(nameof (Constructor)))
        this.Constructor = TypeConstructor.FromString(url[nameof (Constructor)]);
      if (url.ContainsKey(nameof (ShouldSignInFirst)))
        bool.TryParse(url[nameof (ShouldSignInFirst)], out this.ShouldSignInFirst);
      if (!url.ContainsKey(nameof (Play)))
        return;
      bool.TryParse(url[nameof (Play)], out this.Play);
    }

    public TileArgs(string urlConstructor)
      : this(new URLConstructor(urlConstructor))
    {
    }

    public override string ToString()
    {
      URLConstructor urlConstructor = new URLConstructor(nameof (TileArgs));
      if (this.OverCanvasPage != -1)
        urlConstructor["OverCanvasPage"] = this.OverCanvasPage.ToString();
      if (this.PageType != null)
        urlConstructor["PageType"] = this.PageType;
      if (this.Param != null)
        urlConstructor["Param"] = this.Param;
      if (this.Constructor != null)
        urlConstructor["Constructor"] = this.Constructor.ToString();
      urlConstructor["ShouldSignInFirst"] = this.ShouldSignInFirst.ToString();
      if (this.Play)
        urlConstructor["Play"] = this.Play.ToString();
      return urlConstructor.ToString();
    }
  }
}
