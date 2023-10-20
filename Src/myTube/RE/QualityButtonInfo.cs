// Decompiled with JetBrains decompiler
// Type: myTube.QualityButtonInfo
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace myTube
{
  public class QualityButtonInfo : DependencyObject, INotifyPropertyChanged
  {
    private YouTubeQuality qual = YouTubeQuality.LQ;
    private string text = "";
    private string title = "";
    private bool enabled = true;
    private int fontSize = 13;

    public YouTubeQuality Quality
    {
      get => this.qual;
      set
      {
        if (value == this.qual)
          return;
        this.opc(nameof (Quality));
        this.qual = value;
      }
    }

    public string Text
    {
      get => this.text;
      set
      {
        if (!(this.text != value))
          return;
        this.text = value;
        this.opc(nameof (Text));
      }
    }

    public string Title
    {
      get => this.title;
      set
      {
        if (!(this.title != value))
          return;
        this.title = value;
        this.opc(nameof (Title));
      }
    }

    public int FontSize
    {
      get => this.fontSize;
      set
      {
        if (this.fontSize == value)
          return;
        this.fontSize = value;
        this.opc(nameof (FontSize));
      }
    }

    public bool IsEnabled
    {
      get => this.enabled;
      set
      {
        if (this.enabled == value)
          return;
        this.enabled = value;
        this.opc(nameof (IsEnabled));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc([CallerMemberName] string prop = null)
    {
      if (prop == null || this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(prop));
    }
  }
}
