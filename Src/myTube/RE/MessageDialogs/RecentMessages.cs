// Decompiled with JetBrains decompiler
// Type: myTube.MessageDialogs.RecentMessages
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.MessageDialogs
{
  public sealed class RecentMessages : Page, IComponentConnector
  {
    private MessageClient client;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar progress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noMessages;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl items;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public RecentMessages()
    {
      this.InitializeComponent();
      this.client = new MessageClient();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.RecentMessages_Loaded));
    }

    private void RecentMessages_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private async Task LoadMessages()
    {
      this.progress.put_IsIndeterminate(true);
      ((UIElement) this.noMessages).put_Visibility((Visibility) 1);
      try
      {
        Message[] messagesAfter = await this.client.GetMessagesAfter(long.MinValue, 30, Settings.UserMode, App.GlobalObjects.Version);
        if (messagesAfter.Length != 0)
          ((UIElement) this.noMessages).put_Visibility((Visibility) 1);
        else
          ((UIElement) this.noMessages).put_Visibility((Visibility) 0);
        this.items.put_ItemsSource((object) messagesAfter);
      }
      catch
      {
        if (((ICollection<object>) this.items.Items).Count > 0)
          ((UIElement) this.noMessages).put_Visibility((Visibility) 1);
        else
          ((UIElement) this.noMessages).put_Visibility((Visibility) 0);
      }
      this.progress.put_IsIndeterminate(false);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (this.items.ItemsSource != null && e.NavigationMode == 1)
        return;
      this.LoadMessages();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///MessageDialogs/RecentMessages.xaml"), (ComponentResourceLocation) 0);
      this.progress = (ProgressBar) ((FrameworkElement) this).FindName("progress");
      this.noMessages = (TextBlock) ((FrameworkElement) this).FindName("noMessages");
      this.items = (ItemsControl) ((FrameworkElement) this).FindName("items");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
