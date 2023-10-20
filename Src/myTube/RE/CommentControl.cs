// Decompiled with JetBrains decompiler
// Type: myTube.CommentControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
  public sealed class CommentControl : UserControl, IComponentConnector
  {
    private bool replyEnabled = true;
    private static ThumbnailDispatcher thumbnailDispatcher = new ThumbnailDispatcher();
    private string lastId = "";
    private bool likeButtonsShown;
    private bool? liked;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl main;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style likedStyle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage thumbBitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid grid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition likesColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Ellipse thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock authorBlock;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock commentBlock;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock time;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid replyGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl rating;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl likeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl dislikeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox replyBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl replyButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run replySeparator;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run replyRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler<Comment> RepliedTo;

    private Comment Comment => ((FrameworkElement) this).DataContext as Comment;

    public CommentControl()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(CommentControl_DataContextChanged)));
      try
      {
        ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      }
      catch
      {
      }
    }

    private async void CommentControl_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Comment == null)
        return;
      Task<bool> task = (Task<bool>) null;
      if (this.lastId != this.Comment.ID)
      {
        ((UIElement) this.thumb).put_Opacity(0.0);
        task = CommentControl.thumbnailDispatcher.AddData(this.thumbBitmap, this.Comment.ChannelThumb);
      }
      this.lastId = this.Comment.ID;
      this.showLikeButtons(false);
      this.liked = new bool?();
      if (string.IsNullOrWhiteSpace(this.Comment.CommentThreadId) || SharedSettings.CurrentAccount == null)
      {
        bool flag;
        ((UIElement) this.time).put_IsHitTestVisible(flag = false);
        this.replyEnabled = flag;
        this.replySeparator.put_Text("");
        this.replyRun.put_Text("");
      }
      else
      {
        this.replySeparator.put_Text("|");
        this.replyBox.put_Text("");
        this.replyRun.put_Text(App.Strings["common.reply", "reply"]);
        bool flag;
        ((UIElement) this.time).put_IsHitTestVisible(flag = true);
        this.replyEnabled = flag;
      }
      ((UIElement) this.replyGrid).put_Visibility((Visibility) 1);
      if (task == null)
        return;
      int num = await task ? 1 : 0;
      Ani.Begin((DependencyObject) this.thumb, "Opacity", 1.0, 0.3);
    }

    private void authorBlock_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is Comment))
        return;
      (Application.Current as App).RootFrame.Navigate(typeof (ChannelPage), (object) (((FrameworkElement) this).DataContext as Comment).Channel);
    }

    private async void replyButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.Comment == null || string.IsNullOrWhiteSpace(this.Comment.CommentThreadId))
        return;
      if (!string.IsNullOrWhiteSpace(this.replyBox.Text))
      {
        string text = this.replyBox.Text;
        Ani.Begin((DependencyObject) this.replyButton, "Opacity", 0.5, 0.2);
        ((UIElement) this.replyButton).put_IsHitTestVisible(false);
        ((Control) this.replyBox).put_IsEnabled(false);
        Comment comment = (Comment) null;
        try
        {
          UserInfoClient userInfoClient = new UserInfoClient();
          userInfoClient.ClearParts(Part.ContentDetails);
          string plusId = (string) null;
          try
          {
            plusId = (await userInfoClient.GetInfo(this.Comment.Channel)).GooglePlusId;
          }
          catch
          {
          }
          if (plusId == null)
          {
            comment = await CommentClient.Reply(text.Trim(), this.Comment.CommentThreadId);
            comment.Content = text.Trim();
          }
          else
          {
            comment = await CommentClient.Reply("@" + plusId + " " + text.Trim(), this.Comment.CommentThreadId);
            comment.Content = "+" + this.Comment.Author + " " + text.Trim();
          }
          comment.ChannelThumb = YouTube.UserInfo.ThumbUri;
          plusId = (string) null;
        }
        catch
        {
          ((Control) this.replyBox).put_IsEnabled(true);
          Ani.Begin((DependencyObject) this.replyButton, "Opacity", 1.0, 0.2);
          ((UIElement) this.replyButton).put_IsHitTestVisible(true);
          return;
        }
        ((Control) this.replyBox).put_IsEnabled(true);
        Ani.Begin((DependencyObject) this.replyButton, "Opacity", 1.0, 0.2);
        ((UIElement) this.replyButton).put_IsHitTestVisible(true);
        ((UIElement) this.replyGrid).put_Visibility((Visibility) 1);
        this.replyBox.put_Text("");
        this.replyRun.put_Text(App.Strings["common.reply", "reply"].ToLower());
        if (this.RepliedTo != null)
        {
          if (comment == null)
            comment = new Comment();
          this.RepliedTo((object) this, comment);
        }
        text = (string) null;
        comment = (Comment) null;
      }
      else
        ((Control) this.replyBox).Focus((FocusState) 3);
    }

    private async void time_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!this.replyEnabled)
        return;
      if (((UIElement) this.replyGrid).Visibility == null)
      {
        this.replyRun.put_Text(App.Strings["common.reply", "reply"].ToLower());
        ((UIElement) this.replyGrid).put_Visibility((Visibility) 1);
      }
      else
      {
        if (SharedSettings.CurrentAccount == null)
          return;
        if (!SharedSettings.CurrentAccount.Scope.Contains("https://www.googleapis.com/auth/youtube.force-ssl"))
        {
          DefaultPage.Current.OpenBrowser("You need to re-sign into your account to reply to comments. This only has to be done once for this account.");
        }
        else
        {
          this.replyRun.put_Text(App.Strings["common.cancel", "cancel"].ToLower());
          ((UIElement) this.replyGrid).put_Visibility((Visibility) 0);
          ((UIElement) this.replyGrid).put_Opacity(0.0);
          Ani.Begin((DependencyObject) this.replyGrid, "Opacity", 1.0, 0.2);
          await ((FrameworkElement) this.replyGrid).WaitForLayoutUpdateAsync();
          ((Control) this.replyBox).Focus((FocusState) 3);
        }
      }
    }

    private void BitmapImage_ImageOpened(object sender, RoutedEventArgs e)
    {
    }

    private void rating_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.Comment == null || !(this.Comment.Time > new DateTime(2013, 11, 1)) || !YouTube.IsSignedIn)
        return;
      this.showLikeButtons(!this.likeButtonsShown);
    }

    private void showLikeButtons(bool show)
    {
      if (show)
      {
        if (this.likeButtonsShown)
          return;
        if (this.liked.HasValue)
        {
          if (this.liked.Value)
          {
            ((FrameworkElement) this.likeButton).put_Style(this.likedStyle);
            ((FrameworkElement) this.dislikeButton).put_Style((Style) null);
          }
          else
          {
            ((FrameworkElement) this.dislikeButton).put_Style(this.likedStyle);
            ((FrameworkElement) this.likeButton).put_Style((Style) null);
          }
        }
        else
        {
          ContentControl likeButton = this.likeButton;
          Style style1;
          ((FrameworkElement) this.dislikeButton).put_Style(style1 = (Style) null);
          Style style2 = style1;
          ((FrameworkElement) likeButton).put_Style(style2);
        }
        this.likeButtonsShown = true;
        ContentControl likeButton1 = this.likeButton;
        double num1;
        ((UIElement) this.dislikeButton).put_Opacity(num1 = 0.0);
        double num2 = num1;
        ((UIElement) likeButton1).put_Opacity(num2);
        ContentControl likeButton2 = this.likeButton;
        Visibility visibility1;
        ((UIElement) this.dislikeButton).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 0));
        Visibility visibility2 = visibility1;
        ((UIElement) likeButton2).put_Visibility(visibility2);
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.likeButton, "Opacity", 1.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.dislikeButton, "Opacity", 1.0, 0.2));
      }
      else
      {
        if (!this.likeButtonsShown)
          return;
        this.likeButtonsShown = false;
        Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.likeButton, "Opacity", 0.0, 0.1), (Timeline) Ani.DoubleAni((DependencyObject) this.dislikeButton, "Opacity", 0.0, 0.1));
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
        {
          ContentControl likeButton = this.likeButton;
          Visibility visibility3;
          ((UIElement) this.dislikeButton).put_Visibility((Visibility) (int) (visibility3 = (Visibility) 1));
          Visibility visibility4 = visibility3;
          ((UIElement) likeButton).put_Visibility(visibility4);
          ((UIElement) this).InvalidateArrange();
          ((UIElement) this).InvalidateMeasure();
        }));
      }
    }

    private async void likeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.Comment == null)
        return;
      bool undo = false;
      if (this.liked.HasValue && this.liked.Value)
        undo = true;
      ((FrameworkElement) this.likeButton).put_Style(undo ? (Style) null : this.likedStyle);
      ((FrameworkElement) this.dislikeButton).put_Style((Style) null);
      this.hide();
      if (undo)
        --this.Comment.Likes;
      else if (this.liked.HasValue && !this.liked.Value)
        this.Comment.Likes += 2;
      else
        ++this.Comment.Likes;
      try
      {
        int num = await YouTube.RateComment(this.Comment, true, undo) ? 1 : 0;
        this.liked = undo ? new bool?() : new bool?(true);
      }
      catch
      {
      }
      ((UIElement) this.likeButton).put_IsHitTestVisible(true);
      Ani.Begin((DependencyObject) this.likeButton, "Opacity", 1.0, 0.2);
    }

    private async void hide()
    {
      await Task.Delay(750);
      this.showLikeButtons(false);
    }

    private async void dislikeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.Comment == null)
        return;
      bool undo = false;
      if (this.liked.HasValue && !this.liked.Value)
        undo = true;
      ((FrameworkElement) this.dislikeButton).put_Style(undo ? (Style) null : this.likedStyle);
      ((FrameworkElement) this.likeButton).put_Style((Style) null);
      this.hide();
      if (undo)
        ++this.Comment.Likes;
      else if (this.Comment.Likes > 0)
      {
        if (this.liked.HasValue && this.liked.Value)
          this.Comment.Likes -= Math.Min(this.Comment.Likes, 2);
        else
          --this.Comment.Likes;
      }
      try
      {
        int num = await YouTube.RateComment(this.Comment, false, undo) ? 1 : 0;
        this.liked = undo ? new bool?() : new bool?(false);
      }
      catch
      {
      }
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///CommentControl.xaml"), (ComponentResourceLocation) 0);
      this.main = (UserControl) ((FrameworkElement) this).FindName("main");
      this.likedStyle = (Style) ((FrameworkElement) this).FindName("likedStyle");
      this.thumbBitmap = (BitmapImage) ((FrameworkElement) this).FindName("thumbBitmap");
      this.grid = (Grid) ((FrameworkElement) this).FindName("grid");
      this.likesColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("likesColumn");
      this.thumb = (Ellipse) ((FrameworkElement) this).FindName("thumb");
      this.authorBlock = (TextBlock) ((FrameworkElement) this).FindName("authorBlock");
      this.commentBlock = (TextBlock) ((FrameworkElement) this).FindName("commentBlock");
      this.time = (TextBlock) ((FrameworkElement) this).FindName("time");
      this.replyGrid = (Grid) ((FrameworkElement) this).FindName("replyGrid");
      this.rating = (ContentControl) ((FrameworkElement) this).FindName("rating");
      this.likeButton = (ContentControl) ((FrameworkElement) this).FindName("likeButton");
      this.dislikeButton = (ContentControl) ((FrameworkElement) this).FindName("dislikeButton");
      this.replyBox = (TextBox) ((FrameworkElement) this).FindName("replyBox");
      this.replyButton = (ContentControl) ((FrameworkElement) this).FindName("replyButton");
      this.replySeparator = (Run) ((FrameworkElement) this).FindName("replySeparator");
      this.replyRun = (Run) ((FrameworkElement) this).FindName("replyRun");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          BitmapImage bitmapImage = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage.add_ImageOpened), new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), new RoutedEventHandler(this.BitmapImage_ImageOpened));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.authorBlock_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.authorBlock_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.time_Tapped));
          break;
        case 5:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.rating_Tapped));
          break;
        case 6:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.likeButton_Tapped));
          break;
        case 7:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.dislikeButton_Tapped));
          break;
        case 8:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.replyButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
