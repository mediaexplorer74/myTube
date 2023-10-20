// Decompiled with JetBrains decompiler
// Type: myTube.StringFormat
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UriTester;

namespace myTube
{
  public class StringFormat : INotifyPropertyChanged
  {
    private XElement xml;

    public string Name
    {
      get => this.xml.GetAttribute(nameof (Name)).Value;
      set
      {
        if (!(this.Name != value))
          return;
        this.xml.GetAttribute(nameof (Name)).Value = value;
        this.opc(nameof (Name));
      }
    }

    public string Description
    {
      get => this.xml.GetAttribute(nameof (Description)).Value;
      set
      {
        if (!(this.Description != value))
          return;
        this.xml.GetAttribute(nameof (Description)).Value = value;
        this.opc(nameof (Description));
      }
    }

    public string Path
    {
      get => this.xml.GetAttribute(nameof (Path)).Value;
      set
      {
        if (!(this.Path != value))
          return;
        this.xml.GetAttribute(nameof (Path)).Value = value;
        this.opc(nameof (Path));
      }
    }

    public StringCaseType CaseType
    {
      get
      {
        try
        {
          return (StringCaseType) Enum.Parse(typeof (StringCaseType), this.xml.GetAttribute(nameof (CaseType)).Value);
        }
        catch
        {
          return StringCaseType.FirstCapitalized;
        }
      }
      set => this.xml.GetAttribute(nameof (CaseType)).Value = value.ToString();
    }

    public XElement XML => this.xml;

    public StringFormat() => this.xml = new XElement((XName) nameof (StringFormat));

    public StringFormat(XElement Xml) => this.xml = Xml;

    private void opc([CallerMemberName] string p = null)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(p));
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
