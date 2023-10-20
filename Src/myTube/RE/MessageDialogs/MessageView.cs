// Decompiled with JetBrains decompiler
// Type: myTube.MessageDialogs.MessageView
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

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
  public sealed class MessageView : UserControl, IComponentConnector
  {
    private string lastMessageId = "-1";
    private CommentsView commentsView;
    private Popup p;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl commentsContent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl button;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private RichTextBlock commentsText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run commentsNumberRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run commentsTextRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl actionsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel extraContent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public Message Message => ((FrameworkElement) this).DataContext is Message ? (Message) ((FrameworkElement) this).DataContext : (Message) null;

    public MessageView()
    {
      this.InitializeComponent();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(MessageView_DataContextChanged)));
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
        ((FrameworkElement) pollView).put_DataContext((object) poll);
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
          this.commentsNumberRun.put_Text(num.ToString());
          if (num == 1)
            this.commentsTextRun.put_Text(" comment");
          else
            this.commentsTextRun.put_Text(" comments");
        }
        else
        {
          this.commentsNumberRun.put_Text("tap here to comment");
          this.commentsTextRun.put_Text("");
        }
      }
      catch
      {
      }
    }

    private void button_Tapped(object sender, TappedRoutedEventArgs e) => DefaultPage.Current.ClosePopup();

    private void commentsContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Rect bounds1 = Window.Current.Bounds;
      Rect bounds2 = ((FrameworkElement) this.layoutRoot).GetBounds((UIElement) DefaultPage.Current);
      if (this.p == null)
        this.p = new Popup();
      e.GetPosition((UIElement) DefaultPage.Current);
      this.p.put_HorizontalOffset(bounds2.X);
      this.p.put_VerticalOffset(bounds2.Y);
      ((FrameworkElement) this.p).put_Width(bounds2.Width);
      ((FrameworkElement) this.p).put_Height(Math.Min(bounds1.Height, Math.Max(400.0, ((FrameworkElement) this).ActualHeight)));
      if (this.commentsView == null)
      {
        CommentsView commentsView = new CommentsView();
        ((FrameworkElement) commentsView).put_DataContext(((FrameworkElement) this).DataContext);
        ((FrameworkElement) commentsView).put_Width(((FrameworkElement) this.p).Width);
        ((FrameworkElement) commentsView).put_Height(((FrameworkElement) this.p).Height);
        ((FrameworkElement) commentsView).put_HorizontalAlignment((HorizontalAlignment) 3);
        this.commentsView = commentsView;
        this.commentsView.Closed += (EventHandler) ((_param1, _param2) => this.p.put_IsOpen(false));
        this.commentsView.Closing += (EventHandler) ((_param1, _param2) => this.SetCommentsCount());
      }
      ((FrameworkElement) this.p).put_RequestedTheme(App.Theme);
      this.p.put_Child((UIElement) this.commentsView);
      this.p.put_IsOpen(true);
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

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///MessageDialogs/MessageView.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.commentsContent = (ContentControl) ((FrameworkElement) this).FindName("commentsContent");
      this.button = (ContentControl) ((FrameworkElement) this).FindName("button");
      this.commentsText = (RichTextBlock) ((FrameworkElement) this).FindName("commentsText");
      this.commentsNumberRun = (Run) ((FrameworkElement) this).FindName("commentsNumberRun");
      this.commentsTextRun = (Run) ((FrameworkElement) this).FindName("commentsTextRun");
      this.actionsControl = (ItemsControl) ((FrameworkElement) this).FindName("actionsControl");
      this.extraContent = (StackPanel) ((FrameworkElement) this).FindName("extraContent");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.ContentControl_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.commentsContent_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.button_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
