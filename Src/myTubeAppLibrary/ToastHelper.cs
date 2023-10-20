// Decompiled with JetBrains decompiler
// Type: myTube.ToastHelper
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Xml.Linq;
using UriTester;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace myTube
{
  public static class ToastHelper
  {
    public static ToastNotification CreateToast(
      Uri image,
      string text1,
      string text2,
      TileArgs args,
      params ToastAction[] actions)
    {
      XmlDocument templateContent = ToastNotificationManager.GetTemplateContent((ToastTemplateType) 1);
      if (image != (Uri) null)
        templateContent.SetImage(0, image.ToString());
      templateContent.SetText(0, text1);
      templateContent.SetText(1, text2);
      if (args != null)
        ((XmlElement) templateContent.SelectSingleNode("/toast")).SetAttribute("launch", args.ToString());
      if (actions.Length != 0)
      {
        IXmlNode ixmlNode = templateContent.SelectSingleNode("/toast");
        XmlElement element1 = templateContent.CreateElement(nameof (actions));
        ixmlNode.AppendChild((IXmlNode) element1);
        foreach (ToastAction action in actions)
        {
          XmlElement element2 = templateContent.CreateElement("action");
          action.SetXml(element2);
          element1.AppendChild((IXmlNode) element2);
        }
      }
      return new ToastNotification(templateContent);
    }

    public static ToastNotification CreateRedstoneToast(
      Uri image,
      Uri heroImage,
      string text1,
      string text2,
      TileArgs args,
      params ToastAction[] actions)
    {
      XElement x = new XElement((XName) "toast");
      x.Add((object) new XElement((XName) "text")
      {
        Value = text1
      });
      x.Add((object) new XElement((XName) "text")
      {
        Value = text2
      });
      x.SetAttributeValue((XName) "launch", (object) args.ToString());
      XElement element1 = x.GetElement("visual");
      element1.GetElement("binding").GetAttribute("template").Value = "ToastGeneric";
      element1.AddText(text1);
      element1.AddText(text2);
      element1.AddImage(heroImage.OriginalString).SetPlacement(Placement.Hero);
      element1.AddImage(image.OriginalString).SetPlacement(Placement.AppLogoOverride).SetCrop(Crop.Circle);
      if (actions.Length != 0)
      {
        XElement element2 = x.GetElement(nameof (actions));
        foreach (ToastAction action in actions)
          element2.Add((object) action.GetXElement());
      }
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(x.ToString());
      return new ToastNotification(xmlDocument);
    }

    public static ToastNotification CreateToast(string text1, string text2, TileArgs args)
    {
      XmlDocument templateContent = ToastNotificationManager.GetTemplateContent((ToastTemplateType) 5);
      templateContent.SetText(0, text1);
      templateContent.SetText(1, text2);
      if (args != null)
        ((XmlElement) templateContent.SelectSingleNode("/toast")).SetAttribute("launch", args.ToString());
      return new ToastNotification(templateContent);
    }

    public static void AddAction(
      ToastNotification not,
      string text,
      string arguments,
      string activationType,
      string icon)
    {
    }
  }
}
