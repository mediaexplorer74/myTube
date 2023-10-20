// Decompiled with JetBrains decompiler
// Type: myTube.DeviceViewModel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public class DeviceViewModel : INotifyPropertyChanged
  {
    private DeviceInformation device;
    private BitmapImage image;
    private bool selected;

    public BitmapImage Image => this.image;

    public DeviceInformation Device
    {
      get => this.device;
      set => this.SetDevice(value);
    }

    public string Name => this.device != null ? this.device.Name : "DLNA Device";

    public string Id => this.device != null ? this.device.Id : "";

    public bool Selected
    {
      get => this.selected;
      set
      {
        if (this.selected == value)
          return;
        this.selected = value;
        this.opc(nameof (Selected));
      }
    }

    public Uri ThumbUri => new Uri("ms-appx:///Icons/tv.png");

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc([CallerMemberName] string prop = null)
    {
    }

    public DeviceViewModel()
    {
    }

    public DeviceViewModel(DeviceInformation d)
    {
      this.image = new BitmapImage();
      this.SetDevice(d);
    }

    private async void SetDevice(DeviceInformation d)
    {
      this.device = d;
      this.opc("Device");
      this.opc("Name");
      DeviceThumbnail glyphThumbnailAsync = await d.GetGlyphThumbnailAsync();
      if (this.image == null)
      {
        this.image = new BitmapImage();
        this.opc("Image");
      }
      ((BitmapSource) this.image).SetSource((IRandomAccessStream) glyphThumbnailAsync);
    }
  }
}
