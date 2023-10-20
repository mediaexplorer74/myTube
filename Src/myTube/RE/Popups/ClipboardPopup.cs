// Decompiled with JetBrains decompiler
// Type: myTube.Popups.ClipboardPopup
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed class ClipboardPopup : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl closeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ClipboardPopup()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.ClipboardPopup_Tapped));
    }

    private void ClipboardPopup_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      DefaultPage.Current.ClosePopup();
      App.Instance.RootFrame.Navigate(typeof (VideoPage), ((FrameworkElement) this).DataContext);
    }

    private void closeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      DefaultPage.Current.ClosePopup();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/ClipboardPopup.xaml"), (ComponentResourceLocation) 0);
      this.closeButton = (ContentControl) ((FrameworkElement) this).FindName("closeButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.closeButton_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
