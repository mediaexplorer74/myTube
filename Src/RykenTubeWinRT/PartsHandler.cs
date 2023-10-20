// Decompiled with JetBrains decompiler
// Type: RykenTube.PartsHandler
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;

namespace RykenTube
{
  public class PartsHandler
  {
    private List<Part> parts = new List<Part>();

    public List<Part> Parts => this.parts;

    public void ClearParts(params Part[] parts)
    {
      this.parts.Clear();
      this.AddParts(parts);
    }

    public void AddParts(params Part[] parts)
    {
      foreach (Part part in parts)
      {
        if (!this.parts.Contains(part))
          this.parts.Add(part);
      }
    }

    public void RemoveParts(params Part[] parts)
    {
      foreach (Part part in parts)
      {
        if (this.parts.Contains(part))
          this.parts.Remove(part);
      }
    }

    public static string YouTubeEntryPartToString(Part p)
    {
      switch (p)
      {
        case Part.Snippet:
          return "snippet";
        case Part.Statistics:
          return "statistics";
        case Part.ContentDetails:
          return "contentDetails";
        case Part.BrandingSettings:
          return "brandingSettings";
        case Part.Status:
          return "status";
        case Part.Replies:
          return "replies";
        case Part.Targeting:
          return "targeting";
        default:
          return "";
      }
    }

    public string PartsToString() => PartsHandler.YouTubeEntryPartToString((IEnumerable<Part>) this.parts);

    public static string YouTubeEntryPartToString(IEnumerable<Part> parts)
    {
      List<string> values = new List<string>();
      foreach (Part part in parts)
        values.Add(PartsHandler.YouTubeEntryPartToString(part));
      return values.Count > 0 ? string.Join(",", (IEnumerable<string>) values) : "";
    }
  }
}
