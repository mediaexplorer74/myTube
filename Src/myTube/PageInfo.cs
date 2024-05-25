// myTube.PageInfo

using RykenTube;
using System;
using System.Xml.Linq;
using UriTester;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public class PageInfo
  {
    private XElement xml;

    public XElement XML => this.xml;

    public Type PageType
    {
      get
      {
        try
        {
          return Type.GetType(this.xml.GetAttribute(nameof (PageType)).Value);
        }
        catch
        {
          return (Type) null;
        }
      }
      set => this.xml.GetAttribute(nameof (PageType)).Value = value.ToString();
    }

    public Type ParameterType
    {
      get
      {
        try
        {
          return Type.GetType(this.xml.GetAttribute(nameof (ParameterType)).Value);
        }
        catch
        {
          return typeof (string);
        }
      }
      set => this.xml.GetAttribute(nameof (ParameterType)).Value = value.ToString();
    }

    public object Parameter
    {
      get
      {
        string str = this.xml.GetElement(nameof (Parameter)).Value;
        return string.IsNullOrWhiteSpace(str) ? (object) null : PageInfo.GetParam(str, this.ParameterType);
      }
      set => this.xml.GetElement(nameof (Parameter)).Value = PageInfo.SaveParam(value);
    }

    public PageInfo() => this.xml = new XElement((XName) nameof (PageInfo));

    public PageInfo(XElement Xml) => this.xml = Xml;

    public PageInfo(Type type, object param)
      : this()
    {
      this.PageType = type;
      if (param == null)
        return;
      this.Parameter = param;
      this.ParameterType = param.GetType();
    }

    public PageInfo(PageStackEntry page)
      : this(page.SourcePageType, page.Parameter)
    {
    }

    public static string SaveParam(object param)
    {
      switch (param)
      {
        case UserInfo _:
          return (param as UserInfo).ID;
        case YouTubeEntry _:
          return (param as YouTubeEntry).ID;
        case PlaylistEntry _:
          return (param as PlaylistEntry).ID;
        //case VideoPage.ClientConstructorAndEntry _:
        //  return (param as VideoPage.ClientConstructorAndEntry).Entry.ID;
        default:
          return param.ToString();
      }
    }

    public static PageInfo FromShortString(string s)
    {
      PageInfo pageInfo = new PageInfo();
      int length = s.IndexOf(',');
      string str = "";
      string typeName;
      if (length != -1)
      {
        typeName = s.Substring(0, length);
        if (s.Length - 1 > length)
          str = s.Substring(length + 1);
      }
      else
        typeName = s;
      pageInfo.Parameter = (object) str;
      pageInfo.ParameterType = typeof (string);
      try
      {
        pageInfo.PageType = Type.GetType(typeName);
      }
      catch
      {
        //pageInfo.PageType = typeof (HomePage);
      }
      return pageInfo;
    }

    public string ToShortString()
    {
        return string.Format("{0},{1}", (object)this.PageType.FullName, this.Parameter);
    }

    public static object GetParam(string param, Type objType)
    {
      if (objType != typeof (int))
        return (object) param;
      int result = 0;
      return int.TryParse(param, out result) ? (object) result : (object) 0;
    }
  }
}
