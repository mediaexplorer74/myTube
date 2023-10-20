// Decompiled with JetBrains decompiler
// Type: RykenTube.Activity
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace RykenTube
{
  public class Activity : ClientData<Activity>
  {
    private string id;

    public override string ID
    {
      get => this.id;
      set => this.id = value;
    }

    public ActivityItemType ItemType { get; set; } = ActivityItemType.Upload;

    public ActivityFeedType FeedType { get; set; }

    public ActivityReason Reason { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public override bool NeedsRefresh { get; set; }

    public Activity(JToken json) => this.SetValuesFromJson(json);

    public override void SetValuesFromJson(JToken json)
    {
      this.id = json.GetValue<string>((string) null, "id");
      if (!(json[(object) "snippet"] is JObject json1))
        return;
      string[] strArray = new string[1]{ "title" };
      this.Title = json1.GetValue<string>(nameof (Activity), strArray);
    }

    public override void SetValuesFromXML(XElement xml) => throw new NotImplementedException();

    protected override EntryClient<Activity> GetRefreshClientOverride() => throw new NotImplementedException();
  }
}
