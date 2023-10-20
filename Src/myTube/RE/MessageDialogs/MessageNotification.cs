// Decompiled with JetBrains decompiler
// Type: myTube.MessageDialogs.MessageNotification
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

namespace myTube.MessageDialogs
{
  public sealed class MessageNotification : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public MessageNotification()
    {
      this.InitializeComponent();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
    }

    private void Border_Tapped(object sender, TappedRoutedEventArgs e)
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
      Application.LoadComponent((object) this, new Uri("ms-appx:///MessageDialogs/MessageNotification.xaml"), (ComponentResourceLocation) 0);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.Border_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
