// Decompiled with JetBrains decompiler
// Type: myTube.ChannelThumb
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class ChannelThumb : UserControl, IComponentConnector
  {
    private bool uifn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public bool UseIDForNavigation
    {
      get => this.uifn;
      set => this.uifn = value;
    }

    public ChannelThumb() => this.InitializeComponent();

    private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (((FrameworkElement) this).DataContext == null || !(((FrameworkElement) this).DataContext is UserInfo))
        return;
      if (!this.uifn)
        ((App) Application.Current).RootFrame.Navigate(typeof (ChannelPage), ((FrameworkElement) this).DataContext);
      else
        ((App) Application.Current).RootFrame.Navigate(typeof (ChannelPage), (object) (((FrameworkElement) this).DataContext as UserInfo).UserName);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ChannelThumb.xaml"), (ComponentResourceLocation) 0);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.UserControl_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
