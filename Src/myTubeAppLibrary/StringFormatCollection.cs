// Decompiled with JetBrains decompiler
// Type: myTube.StringFormatCollection
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UriTester;

namespace myTube
{
  public class StringFormatCollection : INotifyPropertyChanged
  {
    public static StringFormatCollection GlobalCollection = new StringFormatCollection()
    {
      Path = "",
      Name = "Root",
      Description = "This is the root collection"
    };
    private ObservableCollection<StringFormatCollection> collections;
    private ObservableCollection<StringFormat> stringFormats;
    private string path;
    private string name;
    private string desc;

    static StringFormatCollection() => StringFormatCollection.GlobalCollection.AddStringFormatCollection("common", "Common Strings", "These are common strings that may appear throughout the app");

    public ObservableCollection<StringFormatCollection> Collections => this.collections;

    public ObservableCollection<StringFormat> StringFormats => this.stringFormats;

    public string Path
    {
      get => this.path;
      set
      {
        if (!(this.path != value))
          return;
        this.path = value;
        this.opc(nameof (Path));
      }
    }

    public string Name
    {
      get => this.name;
      set
      {
        if (!(this.name != value))
          return;
        this.name = value;
        this.opc(nameof (Name));
      }
    }

    public string Description
    {
      get => this.desc;
      set
      {
        if (!(this.desc != value))
          return;
        this.desc = value;
        this.opc(nameof (Description));
      }
    }

    public StringFormatCollection()
    {
      this.stringFormats = new ObservableCollection<StringFormat>();
      this.collections = new ObservableCollection<StringFormatCollection>();
    }

    public XElement GetXML()
    {
      XElement x = new XElement((XName) nameof (StringFormatCollection));
      x.GetAttribute("Path").Value = this.Path;
      x.GetAttribute("Name").Value = this.Name;
      x.GetElement("Description").Value = this.Description;
      foreach (StringFormatCollection collection in (Collection<StringFormatCollection>) this.collections)
        x.Add((object) collection.GetXML());
      foreach (StringFormat stringFormat in (Collection<StringFormat>) this.stringFormats)
        x.Add((object) stringFormat.XML);
      return x;
    }

    public void FromXML(XElement xml)
    {
      this.collections.Clear();
      this.Path = xml.GetAttribute("Path").Value;
      this.Name = xml.GetAttribute("Name").Value;
      this.Description = xml.GetElement("Description").Value;
      List<StringFormatCollection> source1 = new List<StringFormatCollection>();
      foreach (XElement element in xml.Elements((XName) nameof (StringFormatCollection)))
      {
        StringFormatCollection formatCollection = new StringFormatCollection();
        formatCollection.FromXML(element);
        source1.Add(formatCollection);
      }
      this.collections.Add<StringFormatCollection>((IList<StringFormatCollection>) source1.OrderBy<StringFormatCollection, string>((Func<StringFormatCollection, string>) (s1 => s1.Name)).ToList<StringFormatCollection>());
      this.stringFormats.Clear();
      List<StringFormat> source2 = new List<StringFormat>();
      foreach (XElement element in xml.Elements((XName) "StringFormat"))
      {
        StringFormat stringFormat = new StringFormat(element);
        source2.Add(stringFormat);
      }
      this.stringFormats.Add<StringFormat>((IList<StringFormat>) source2.OrderBy<StringFormat, string>((Func<StringFormat, string>) (ssf => ssf.Name)).ToList<StringFormat>());
    }

    public void DeleteStringFormatCollection(StringFormatCollection sfc)
    {
      if (!this.collections.Contains(sfc))
        return;
      this.collections.Remove(sfc);
    }

    public void AddStringFormatCollection(string path, string name, string desc)
    {
      string str = (string.IsNullOrWhiteSpace(this.Path) ? "" : this.Path + ".") + path;
      foreach (StringFormatCollection collection in (Collection<StringFormatCollection>) this.collections)
      {
        if (collection.Name == name || collection.Path == str)
          throw new InvalidOperationException("This string format collection already exists");
      }
      this.collections.Add(new StringFormatCollection()
      {
        Path = str,
        Name = name,
        Description = desc
      });
    }

    public void AddStringFormat(StringFormat sf)
    {
      StringFormat stringFormat1 = (StringFormat) null;
      foreach (StringFormat stringFormat2 in (Collection<StringFormat>) this.stringFormats)
      {
        if (stringFormat2.Name == sf.Name || stringFormat2.Path == sf.Path)
          stringFormat1 = stringFormat2;
      }
      if (stringFormat1 != null)
        this.stringFormats.Remove(stringFormat1);
      if (this.stringFormats.Contains(sf))
        return;
      this.stringFormats.Add(sf);
    }

    public void DeleteStringFormat(StringFormat sf)
    {
      if (!this.stringFormats.Contains(sf))
        return;
      this.stringFormats.Remove(sf);
    }

    public bool AllStringsTranslated(Strings strings)
    {
      foreach (StringFormatCollection collection in (Collection<StringFormatCollection>) this.collections)
      {
        if (!collection.AllStringsTranslated(strings))
          return false;
      }
      foreach (StringFormat stringFormat in (Collection<StringFormat>) this.stringFormats)
      {
        if (strings[stringFormat.Path] == null)
          return false;
      }
      return true;
    }

    public void TrimAllStrings(Strings strings)
    {
      foreach (StringFormat stringFormat in (Collection<StringFormat>) this.stringFormats)
      {
        if (strings[stringFormat.Path] != null)
          strings[stringFormat.Path] = strings[stringFormat.Path].Trim();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc([CallerMemberName] string p = null)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(p));
    }
  }
}
