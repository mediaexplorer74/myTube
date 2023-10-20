// Decompiled with JetBrains decompiler
// Type: RykenTube.ClientDataBase
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public abstract class ClientDataBase : INotifyPropertyChanged
  {
    private Dictionary<string, object> properties;

    public bool ShouldRefreshList { get; set; }

    public abstract bool NeedsRefresh { get; set; }

    public abstract string ID { get; set; }

    public abstract void SetValuesFromXML(XElement xml);

    public abstract void SetValuesFromJson(JToken json);

    public event PropertyChangedEventHandler PropertyChanged;

    public async Task Refresh()
    {
      if (!this.NeedsRefresh)
        return;
      this.NeedsRefresh = false;
      try
      {
        await this.RefreshOverride();
      }
      catch
      {
        this.NeedsRefresh = true;
      }
    }

    protected abstract Task RefreshOverride();

    protected void OnPropertyChanged([CallerMemberName] string property = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(property));
    }

    protected void opc([CallerMemberName] string prop = null)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(prop));
    }

    public T GetProperty<T>(string key) => this.properties != null && this.properties.ContainsKey(key) && this.properties[key] is T ? (T) this.properties[key] : default (T);

    public void SetProperty(string key, object obj)
    {
      if (this.properties == null)
        this.properties = new Dictionary<string, object>();
      if (this.properties.ContainsKey(key))
        this.properties[key] = obj;
      else
        this.properties.Add(key, obj);
    }

    public bool HasProperty<T>(string key) => this.properties != null && this.properties.ContainsKey(key) && this.properties[key] is T;

    public bool HasProperty(string key) => this.properties != null && this.properties.ContainsKey(key);
  }
}
