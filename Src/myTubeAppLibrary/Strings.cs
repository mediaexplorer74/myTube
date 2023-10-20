// Decompiled with JetBrains decompiler
// Type: myTube.Strings
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace myTube
{
  public class Strings : INotifyPropertyChanged
  {
    public const string FileName = "Strings.xml";
    private XElement xml;
    private string vals = "value";

    public XElement XML => this.xml;

    public string this[string s]
    {
      get
      {
        int length = s.IndexOf(',');
        string str1 = (string) null;
        if (length != -1)
        {
          str1 = s.Substring(length + 1);
          s = s.Substring(0, length);
        }
        if (this.xml == null)
          return str1;
        XElement element = this.xml.GetElement(s);
        string str2 = element.Value;
        if (!string.IsNullOrWhiteSpace(str2))
          return str2;
        element.Remove();
        return str1;
      }
      set
      {
        this.xml.GetElement(s).Value = value;
        this.opc();
      }
    }

    public string this[string path, string fallback] => this[path] ?? fallback;

    public string Values
    {
      get => this.vals;
      set => this.vals = value;
    }

    public Strings() => this.xml = new XElement((XName) nameof (Strings));

    public Strings(XElement Xml) => this.xml = Xml;

    public void SetXML(XElement x)
    {
      this.xml = x;
      this.opc();
    }

    public async Task LoadXML(string path = "Strings.xml")
    {
      try
      {
        this.xml = XElement.Parse(await FileIO.ReadTextAsync(
            (IStorageFile) await ApplicationData.Current.LocalFolder.GetFileAsync(path)));
        this.opc();
      }
      catch
      {
      }
    }

    public static async Task<Strings> FromFile(string path = "Strings.xml")
    {
      try
      {
        return new Strings(XElement.Parse(await FileIO.ReadTextAsync((IStorageFile) await ApplicationData.Current.LocalFolder.GetFileAsync(path))));
      }
      catch
      {
        return new Strings();
      }
    }

    public async Task SaveFile(string path = "Strings.xml") => await FileIO.WriteTextAsync((IStorageFile) await ApplicationData.Current.LocalFolder.CreateFileAsync(path, (CreationCollisionOption) 1), this.xml.ToString());

    private void opc(string prop)
    {
            //RnD
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      //Strings.\u003C\u003Ec__DisplayClass19_0 cDisplayClass190 = new Strings.\u003C\u003Ec__DisplayClass19_0();
      // ISSUE: reference to a compiler-generated field
      //cDisplayClass190.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      //cDisplayClass190.prop = prop;

      if (this.PropertyChanged == null)
        return;
      
      // ISSUE: method pointer
      //CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync((CoreDispatcherPriority)(-1), 
      //    new DispatchedHandler((object) cDisplayClass190, __methodptr(\u003Copc\u003Eb__0)));
    }

    private void opc() => this.opc("Values");

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
