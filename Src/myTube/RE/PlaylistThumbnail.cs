// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistThumbnail
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public sealed class PlaylistThumbnail : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage thumbImage;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Image image;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public PlaylistThumbnail()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(PlaylistThumbnail_DataContextChanged)));
    }

    private async void PlaylistThumbnail_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      PlaylistEntry dataContext = ((FrameworkElement) this).DataContext as PlaylistEntry;
    }

    private void layoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry))
        return;
      ((App) Application.Current).RootFrame.Navigate(typeof (PlaylistPage), (object) (((FrameworkElement) this).DataContext as PlaylistEntry));
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlaylistThumbnail.xaml"), (ComponentResourceLocation) 0);
      this.thumbImage = (BitmapImage) ((FrameworkElement) this).FindName("thumbImage");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.image = (Image) ((FrameworkElement) this).FindName("image");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.layoutRoot_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
