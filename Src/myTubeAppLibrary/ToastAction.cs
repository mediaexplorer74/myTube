// Decompiled with JetBrains decompiler
// Type: myTube.ToastAction
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System.Xml.Linq;
using UriTester;
using Windows.Data.Xml.Dom;

namespace myTube
{
  public struct ToastAction
  {
    public ToastActivationType ActivationType;
    public string Content;
    public string Arguments;
    public string Icon;

    public ToastAction(
      string content,
      string arguments,
      ToastActivationType activationType,
      string icon)
    {
      this.Content = content;
      this.Arguments = arguments;
      this.ActivationType = activationType;
      this.Icon = icon;
    }

    public ToastAction(
      string content,
      URLConstructor arguments,
      ToastActivationType activationType,
      string icon)
    {
      this.Content = content;
      this.Arguments = arguments.ToString();
      this.ActivationType = activationType;
      this.Icon = icon;
    }

    public XElement GetXElement()
    {
      XElement x = new XElement((XName) "action");
      if (this.Arguments != null)
        x.GetAttribute("arguments").Value = this.Arguments;
      if (this.Content != null)
        x.GetAttribute("content").Value = this.Content;
      if (this.Icon != null)
        x.GetAttribute("icon").Value = this.Icon;
      switch (this.ActivationType)
      {
        case ToastActivationType.Background:
          x.GetAttribute("activationType").Value = "background";
          break;
        case ToastActivationType.Foreground:
          x.GetAttribute("activationType").Value = "foreground";
          break;
        case ToastActivationType.Protocol:
          x.GetAttribute("activationType").Value = "protocol";
          break;
      }
      return x;
    }

    public void SetXml(XmlElement action)
    {
      if (this.Arguments != null)
        action.SetAttribute("arguments", this.Arguments);
      if (this.Content != null)
        action.SetAttribute("content", this.Content);
      if (this.Icon != null)
        action.SetAttribute("icon", this.Icon);
      switch (this.ActivationType)
      {
        case ToastActivationType.Background:
          action.SetAttribute("activationType", "background");
          break;
        case ToastActivationType.Foreground:
          action.SetAttribute("activationType", "foreground");
          break;
        case ToastActivationType.Protocol:
          action.SetAttribute("activationType", "protocol");
          break;
      }
    }
  }
}
