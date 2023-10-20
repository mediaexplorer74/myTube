// Decompiled with JetBrains decompiler
// Type: myTube.TransferInfo
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;
using Windows.Storage;

namespace myTube
{
  public class TransferInfo
  {
    private XElement xml;

    public XElement XML => this.xml;

    public string Title
    {
      get => this.xml.GetAttribute(nameof (Title)).Value;
      set => this.xml.GetAttribute(nameof (Title)).Value = value;
    }

    public string AuthorDisplayName
    {
      get => this.xml.GetAttribute(nameof (AuthorDisplayName)).Value;
      set => this.xml.GetAttribute(nameof (AuthorDisplayName)).Value = value;
    }

    public string ID
    {
      get => this.xml.GetAttribute(nameof (ID)).Value;
      set => this.xml.GetAttribute(nameof (ID)).Value = value;
    }

    public Uri VideoFilePath
    {
      get
      {
        try
        {
          return new Uri(this.xml.GetElement(nameof (VideoFilePath)).Value, UriKind.RelativeOrAbsolute);
        }
        catch
        {
          return (Uri) null;
        }
      }
      set => this.xml.GetElement(nameof (VideoFilePath)).Value = value == (Uri) null ? "" : value.OriginalString;
    }

    public Uri AudioFilePath
    {
      get
      {
        try
        {
          return new Uri(this.xml.GetElement(nameof (AudioFilePath)).Value, UriKind.RelativeOrAbsolute);
        }
        catch
        {
          return (Uri) null;
        }
      }
      set => this.xml.GetElement(nameof (AudioFilePath)).Value = value == (Uri) null ? "" : value.OriginalString;
    }

    public YouTubeQuality? VideoQuality
    {
      get
      {
        try
        {
          return new YouTubeQuality?((YouTubeQuality) Enum.Parse(typeof (YouTubeQuality), this.xml.GetElement(nameof (VideoQuality)).Value));
        }
        catch
        {
          return new YouTubeQuality?();
        }
      }
      set => this.xml.GetElement(nameof (VideoQuality)).Value = value.ToString();
    }

    public async Task<StorageFile> GetVideoStorageFile()
    {
      int num;
      if (num != 0 && (!(this.VideoFilePath != (Uri) null) || string.IsNullOrEmpty(this.VideoFilePath.OriginalString)))
        return (StorageFile) null;
      try
      {
        return await StorageFile.GetFileFromPathAsync(this.VideoFilePath.OriginalString);
      }
      catch
      {
        return (StorageFile) null;
      }
    }

    public bool IsAdaptive
    {
      get => this.xml.GetBool("Adaptive");
      set => this.xml.SetBool("Adaptive", value);
    }

    public async Task<StorageFile> GetAudioStorageFile()
    {
      int num;
      if (num != 0 && (!(this.AudioFilePath != (Uri) null) || string.IsNullOrEmpty(this.AudioFilePath.OriginalString)))
        return (StorageFile) null;
      try
      {
        return await StorageFile.GetFileFromPathAsync(this.AudioFilePath.OriginalString);
      }
      catch
      {
        return (StorageFile) null;
      }
    }

    public string VideoGUID
    {
      get => this.xml.GetElement("GUID").Value;
      set => this.xml.GetElement("GUID").Value = value == null ? "" : value;
    }

    public string AudioGUID
    {
      get => this.xml.GetElement(nameof (AudioGUID)).Value;
      set => this.xml.GetElement(nameof (AudioGUID)).Value = value == null ? "" : value;
    }

    public TransferInfo() => this.xml = new XElement((XName) nameof (TransferInfo));

    public TransferInfo(YouTubeEntry ent)
      : this()
    {
      this.Title = ent.Title;
      this.AuthorDisplayName = ent.AuthorDisplayName;
      this.ID = ent.ID;
    }

    public TransferInfo(XElement xml) => this.xml = xml;
  }
}
