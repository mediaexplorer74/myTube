// Decompiled with JetBrains decompiler
// Type: myTube.Popups.ChannelNotificationViewModel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace myTube.Popups
{
  public class ChannelNotificationViewModel : INotifyPropertyChanged
  {
    private bool notify;
    private string id;
    private string name;

    public bool Notify
    {
      get => App.GlobalObjects.ChannelNotifications != null ? App.GlobalObjects.ChannelNotifications.ContainsChannel(this.id) : this.notify;
      set
      {
        this.notify = value;
        if (App.GlobalObjects.ChannelNotifications != null)
        {
          if (!value)
            App.GlobalObjects.ChannelNotifications.RemoveChannel(this.id);
          else
            App.GlobalObjects.ChannelNotifications.AddChannel(this.id, this.name);
        }
        this.opc(nameof (Notify));
      }
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    public ChannelNotificationViewModel()
    {
    }

    public ChannelNotificationViewModel(string id, string name)
    {
      this.id = id;
      this.name = name;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc([CallerMemberName] string prop = null)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(prop));
    }
  }
}
