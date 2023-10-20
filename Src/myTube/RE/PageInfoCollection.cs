// Decompiled with JetBrains decompiler
// Type: myTube.PageInfoCollection
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.Linq;
using System.Xml.Linq;

namespace myTube
{
  public class PageInfoCollection
  {
    private XElement xml;

    public XElement XML => this.xml;

    public PageInfo[] Pages
    {
      get
      {
        try
        {
          XElement[] array = Enumerable.ToArray<XElement>(this.xml.Elements((XName) "PageInfo"));
          PageInfo[] pages = new PageInfo[array.Length];
          for (int index = 0; index < pages.Length; ++index)
            pages[index] = new PageInfo(array[index]);
          return pages;
        }
        catch
        {
          return new PageInfo[0];
        }
      }
    }

    public PageInfoCollection() => this.xml = new XElement((XName) nameof (PageInfoCollection));

    public PageInfoCollection(XElement Xml) => this.xml = Xml;

    public static PageInfoCollection FromShortStringUrl(URLConstructor url)
    {
      PageInfoCollection pageInfoCollection = new PageInfoCollection();
      for (int index = 0; index < 1000 && url.ContainsKey(index.ToString()); ++index)
        pageInfoCollection.AddPage(PageInfo.FromShortString(url[index.ToString()]));
      return pageInfoCollection;
    }

    public void AddPage(PageInfo page) => this.xml.Add((object) page.XML);
  }
}
