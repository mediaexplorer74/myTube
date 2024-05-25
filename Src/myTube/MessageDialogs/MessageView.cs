// myTube.MessageDialogs.MessageView


using myTube.Cloud;
using myTube.Cloud.Clients;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.MessageDialogs
{
  public sealed partial class MessageView : UserControl
  {
    private string lastMessageId = "-1";
    private CommentsView commentsView;
    private Popup p;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl commentsContent;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl button;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private RichTextBlock commentsText;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run commentsNumberRun;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run commentsTextRun;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl actionsControl;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel extraContent;
       

        public Message Message
        {
            get
            {
                return ((FrameworkElement)this).DataContext is Message
                    ? (Message)((FrameworkElement)this).DataContext 
                    : (Message)null;
            }
        }

        public MessageView()
    {
      //this.InitializeComponent();
      ((Control) this).FontFamily = ((Control) DefaultPage.Current).FontFamily;
       
         //RnD
        ((FrameworkElement)this).DataContextChanged += MessageView_DataContextChanged;
        ((FrameworkElement)this).DataContextChanged -= MessageView_DataContextChanged;
        }

    private async void MessageView_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is Message))
        return;
      Message dataContext = (Message) ((FrameworkElement) this).DataContext;
      if (dataContext.Id == this.lastMessageId)
        return;
      this.lastMessageId = dataContext.Id;
      this.SetCommentsCount();
      if (!(dataContext.PollId != "-1"))
        return;
      try
      {
        PollData poll = await new PollDataClient().GetPoll(dataContext.PollId);
        if (poll == null)
          return;
        UIElementCollection children = ((Panel) this.extraContent).Children;
        PollView pollView = new PollView();
        ((FrameworkElement) pollView).DataContext = (object) poll;
        ((ICollection<UIElement>) children).Add((UIElement) pollView);
      }
      catch (Exception ex)
      {
      }
    }

    private async Task SetCommentsCount()
    {
      if (this.Message == null)
        return;
      myTube.Cloud.Clients.CommentClient commentClient = new myTube.Cloud.Clients.CommentClient();
      try
      {
        int num = await commentClient.Count(this.Message.Id);
        if (num > 0)
        {
          this.commentsNumberRun.Text = num.ToString();
          if (num == 1)
            this.commentsTextRun.Text = " comment";
          else
            this.commentsTextRun.Text = " comments";
        }
        else
        {
          this.commentsNumberRun.Text = "tap here to comment";
          this.commentsTextRun.Text = "";
        }
      }
      catch
      {
      }
    }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //DefaultPage.Current.ClosePopup();
        }

        private void commentsContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Rect bounds1 = Window.Current.Bounds;
      Rect bounds2 = ((FrameworkElement) this.layoutRoot).GetBounds((UIElement) DefaultPage.Current);
      if (this.p == null)
        this.p = new Popup();
      e.GetPosition((UIElement) DefaultPage.Current);

      this.p.HorizontalOffset = bounds2.X;
      this.p.VerticalOffset = bounds2.Y;

      ((FrameworkElement) this.p).Width = bounds2.Width;
      ((FrameworkElement) this.p).Height = Math.Min(bounds1.Height, 
             Math.Max(400.0, ((FrameworkElement) this).ActualHeight));

      if (this.commentsView == null)
      {
        CommentsView commentsView = new CommentsView();
        commentsView.DataContext = (((FrameworkElement) this).DataContext);
        commentsView.Width = (((FrameworkElement) this.p).Width);
        commentsView.Height = (((FrameworkElement) this.p).Height);
        commentsView.HorizontalAlignment = ((HorizontalAlignment) 3);
        this.commentsView = commentsView;
        this.commentsView.Closed += (EventHandler) ((_param1, _param2) => this.p.IsOpen = false);
        this.commentsView.Closing += (EventHandler) ((_param1, _param2) => this.SetCommentsCount());
      }
      ((FrameworkElement) this.p).RequestedTheme = App.Theme;
      this.p.Child = ((UIElement) this.commentsView);
      this.p.IsOpen = true;
    }

    private async void ContentControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      if (frameworkElement.DataContext is MessageAction action)
      {
        switch (action.Type)
        {
          case MessageActionType.URL:
            try
            {
              int num = await Launcher.LaunchUriAsync(new Uri(action.Data)) ? 1 : 0;
              break;
            }
            catch
            {
              break;
            }
          case MessageActionType.Video:
            App.Instance.RootFrame.Navigate(typeof (VideoPage), (object) action.Data);
            break;
          case MessageActionType.Channel:
            App.Instance.RootFrame.Navigate(typeof (ChannelPage), (object) action.Data);
            break;
          case MessageActionType.Store:
            URLConstructor urlConstructor = new URLConstructor("ms-windows-store://pdp/?");
            string str = "9wzdncrcwf3l";
            if (Settings.UserMode == UserMode.Beta)
              str = "9wzdncrdt29j";
            if (!string.IsNullOrWhiteSpace(action.Data))
              str = action.Data;
            urlConstructor["ProductId"] = str;
            int num1 = await Launcher.LaunchUriAsync(urlConstructor.ToUri(UriKind.Absolute)) ? 1 : 0;
            break;
        }
      }
      action = (MessageAction) null;
    }

    
  }
}
