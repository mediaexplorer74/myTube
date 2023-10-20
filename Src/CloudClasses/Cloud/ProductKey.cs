// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.ProductKey
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace myTube.Cloud
{
  public class ProductKey : DataObject
  {
    public string Product { get; set; }

    public string UserId { get; set; }

    public string Key { get; set; }

    [DefaultValue(false)]
    public bool Rejected { get; set; }

    public string Description { get; set; }

    public List<string> DevicesUsedOn { get; set; }

    [DefaultValue(0)]
    public int Activations { get; set; }

    public ProductKey() => this.DevicesUsedOn = new List<string>();
  }
}
