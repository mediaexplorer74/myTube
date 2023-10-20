// Decompiled with JetBrains decompiler
// Type: UriTester.XMLHelper
// Assembly: XmlHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 98FA6677-2889-4129-9314-1B3B5EDBF73B
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\XmlHelper.dll

using System.Xml.Linq;

namespace UriTester
{
  public static class XMLHelper
  {
    public static XElement GetElement(this XElement x, string path)
    {
      string str = path;
      char[] chArray = new char[1]{ '.' };
      foreach (string name in str.Split(chArray))
      {
        if (x == null)
          return (XElement) null;
        XElement xelement = x.Element((XName) name);
        if (xelement != null)
        {
          x = xelement;
        }
        else
        {
          XElement content = new XElement((XName) name);
          x.Add((object) content);
          x = content;
        }
      }
      return x != null ? x : (XElement) null;
    }

    public static XAttribute GetAttribute(this XElement x, string name)
    {
      XAttribute attribute = x.Attribute((XName) name);
      if (attribute != null)
        return attribute;
      XAttribute content = new XAttribute((XName) name, (object) "");
      x.Add((object) content);
      return content;
    }

    public static bool GetBool(this XElement x, string name, bool defaultValue = false)
    {
      XAttribute xattribute = x.Attribute((XName) name);
      if (xattribute == null)
        return defaultValue;
      string str = xattribute.Value;
      return xattribute.Value == "true";
    }

    public static void SetBool(this XElement x, string name, bool value)
    {
      XAttribute xattribute = x.Attribute((XName) name);
      string str = "false";
      if (value)
        str = "true";
      if (xattribute != null)
        xattribute.Value = str;
      else
        x.Add((object) new XAttribute((XName) name, (object) str));
    }
  }
}
