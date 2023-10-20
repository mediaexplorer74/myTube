// Decompiled with JetBrains decompiler
// Type: myTube.TimeBookmark
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace myTube
{
  public class TimeBookmark : INotifyPropertyChanged
  {
    private string id;
    private TimeSpan time = TimeSpan.Zero;

    public event PropertyChangedEventHandler PropertyChanged;

    public string ID
    {
      get => this.id;
      set
      {
        if (!(this.id != value))
          return;
        this.id = value;
        this.opc(nameof (ID));
      }
    }

    public TimeSpan Time
    {
      get => this.time;
      set
      {
        if (!(this.time != value))
          return;
        this.time = value;
        this.opc(nameof (Time));
      }
    }

    public TimeBookmark()
    {
    }

    public TimeBookmark(string videoID, TimeSpan initialTime)
    {
      this.id = videoID;
      this.time = initialTime;
    }

    private void opc([CallerMemberName] string propName = "")
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propName));
    }
  }
}
