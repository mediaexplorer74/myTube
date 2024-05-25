// myTube.Cloud.ProductKey

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
