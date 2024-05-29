// myTube.MessageDialogs.RecentMessages

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
  public sealed partial class RecentMessages : Page
  {
    private MessageClient client;

    //TEMP
  
    private ProgressBar progress = new ProgressBar();

    private TextBlock noMessages = new TextBlock();

    private ItemsControl items = new ItemsControl();


    public RecentMessages()
    {
        //this.InitializeComponent();
        this.client = new MessageClient();

        this.Loaded += this.RecentMessages_Loaded;
    }

    private void RecentMessages_Loaded(object sender, RoutedEventArgs e)
    {
       //
    }

    private async Task LoadMessages()
    {
      this.progress.IsIndeterminate = true;
      ((UIElement) this.noMessages).Visibility = ((Visibility) 1);
      try
      {
        Message[] messagesAfter = await this.client.GetMessagesAfter(long.MinValue, 30, 
            Settings.UserMode, App.GlobalObjects.Version);
        if (messagesAfter.Length != 0)
          ((UIElement) this.noMessages).Visibility = ((Visibility) 1);
        else
          ((UIElement) this.noMessages).Visibility = ((Visibility) 0);
        this.items.ItemsSource = ((object) messagesAfter);
      }
      catch
      {
        if (((ICollection<object>) this.items.Items).Count > 0)
          ((UIElement) this.noMessages).Visibility = ((Visibility) 1);
        else
          ((UIElement) this.noMessages).Visibility = ((Visibility) 0);
      }
      this.progress.IsIndeterminate = (false);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (this.items.ItemsSource != null && e.NavigationMode == NavigationMode.Back)
        return;
      this.LoadMessages();
    }
    
  }
}
