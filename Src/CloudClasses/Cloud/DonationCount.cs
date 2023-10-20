// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.DonationCount
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using System;

namespace myTube.Cloud
{
  public class DonationCount : DataObject
  {
    public DateTime LastDonationAt { get; set; } = DateTime.MinValue;

    public double DonationAmount { get; set; }

    public double DonationTarget { get; set; }

    public double DonationCarryOverAmount { get; set; }

    [JsonIgnore]
    public double TotalDonation => this.DonationAmount + this.DonationCarryOverAmount;

    public int Count { get; set; }
  }
}
