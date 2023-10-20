// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.BetaApp
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace myTube.Cloud
{
  public class BetaApp : DataObject
  {
    public string AppName { get; set; }

    public string Description { get; set; }

    public string Publisher { get; set; }

    [DefaultValue(9000)]
    public int MaxUsers { get; set; }

    public string Logo { get; set; }

    public List<string> Screenshots { get; set; }

    public string DownloadLink { get; set; }

    public string StoreId { get; set; }

    public string OwnerId { get; set; }

    public BetaApp() => this.Screenshots = new List<string>();
  }
}
