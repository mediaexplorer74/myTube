// myTube.History

using RykenTube;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace myTube
{
  public class History
  {
    private const int MaxVideos = 50;
    public const string SavePath = "History.xml";
    private List<YouTubeEntry> entries;

    private static StorageFolder Folder => ApplicationData.Current.LocalFolder;

    public History() => this.entries = new List<YouTubeEntry>();

    public History(XElement Xml)
    {
      this.entries = History.ExtractVideos(Xml);
      if (this.entries != null)
        return;
      this.entries = new List<YouTubeEntry>();
    }

    public async Task<YouTubeEntry[]> GetEntries(int page)
    {
      int num1 = 10;
      int num2 = num1 * page;
      int length = 10;
      int num3 = this.entries.Count - num2;
      if (num3 <= 0)
        return new YouTubeEntry[0];
      if (num3 < num1)
        length = num3;
      YouTubeEntry[] entries = new YouTubeEntry[length];
      for (int index = 0; index < entries.Length; ++index)
        entries[index] = this.entries[num2 + index];
      return entries;
    }

    public void Clear() => this.entries.Clear();

    public void AddEntry(YouTubeEntry entry)
    {
      string id = entry.ID;
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ID == id)
        {
          this.entries.RemoveAt(index);
          break;
        }
      }
      this.entries.Insert(0, entry);
      while (this.entries.Count > 50)
        this.entries.RemoveAt(this.entries.Count - 1);
    }

    public bool HasEntry(string id)
    {
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ID == id)
          return true;
      }
      return false;
    }

    public bool RemoveEntry(YouTubeEntry entry)
    {
      string id = entry.ID;
      for (int index = 0; index < this.entries.Count; ++index)
      {
        if (this.entries[index].ID == id)
        {
          this.entries.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public static List<YouTubeEntry> ExtractVideos(XElement xml)
    {
      try
      {
        IEnumerable<XElement> xelements = xml.Elements((XName) "entry");
        List<YouTubeEntry> videos = new List<YouTubeEntry>();
        foreach (XElement xelement in xelements)
        {
          try
          {
            videos.Add(new YouTubeEntry(xelement.Value));
          }
          catch
          {
          }
        }
        return videos;
      }
      catch
      {
        return (List<YouTubeEntry>) null;
      }
    }

    public Task Save() => History.Save(this, "History.xml");

    public Task Save(string path) => History.Save(this, path);

    public static XElement CreateXML(History history)
    {
      XElement xml = new XElement((XName) nameof (History));
      foreach (YouTubeEntry entry in history.entries)
        xml.Add((object) new XElement((XName) "entry")
        {
          Value = entry.OriginalString
        });
      return xml;
    }

    public static Task Save(History history, string path) => History.Save(History.CreateXML(history), path);

    public static async Task Save(XElement xml, string path)
    {
      string content = xml.ToString();
      await FileIO.WriteTextAsync((IStorageFile) await History.Folder.CreateFileAsync(path, (CreationCollisionOption) 1), content);
    }

    public static async Task<History> Load() => await History.Load("History.xml");

    private static async Task<XElement> LoadXML() => await History.LoadXML("History.xml");

    private static async Task<XElement> LoadXML(string path) => XElement.Parse(await FileIO.ReadTextAsync((IStorageFile) await History.Folder.GetFileAsync(path)));

    public static async Task<History> Load(string path) => new History(await History.LoadXML(path));
  }
}
