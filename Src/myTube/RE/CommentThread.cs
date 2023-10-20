// Decompiled with JetBrains decompiler
// Type: myTube.CommentThread
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
  public sealed class CommentThread : UserControl, IComponentConnector
  {
    public static DependencyProperty RepliesShownProperty = DependencyProperty.Register(nameof (RepliesShown), typeof (bool), typeof (CommentThread), new PropertyMetadata((object) true, new PropertyChangedCallback(CommentThread.OnRepliesShownChanged)));
    private bool lastShowHide = true;
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style showRepliesStyle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform repliesTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressRing progress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl replyList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Border replyTextBorder;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CommentControl mainControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock replyText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnRepliesShownChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as CommentThread).showOrHide();
    }

    public bool RepliesShown
    {
      get => (bool) ((DependencyObject) this).GetValue(CommentThread.RepliesShownProperty);
      set => ((DependencyObject) this).SetValue(CommentThread.RepliesShownProperty, (object) value);
    }

    private Comment Comment => ((FrameworkElement) this).DataContext as Comment;

    public CommentThread()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(CommentThread_DataContextChanged)));
    }

    private void CommentThread_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Comment == null)
        return;
      if (this.RepliesShown)
        this.RepliesShown = false;
      else
        this.showOrHide();
      this.setVisibilities();
    }

    private void setVisibilities()
    {
      if (this.Comment == null)
        return;
      if (this.Comment.Replies == null || this.Comment.Replies.Count <= 1)
      {
        ((UIElement) this.replyText).put_Visibility((Visibility) 1);
        if (this.Comment.Replies == null || this.Comment.Replies.Count <= 0)
        {
          ((UIElement) this.replyTextBorder).put_Visibility((Visibility) 1);
          this.replyList.put_ItemsSource((object) null);
        }
        else
          ((UIElement) this.replyTextBorder).put_Visibility((Visibility) 0);
        ((UIElement) this.replyTextBorder).put_IsHitTestVisible(false);
      }
      else
      {
        ((UIElement) this.replyTextBorder).put_IsHitTestVisible(true);
        TextBlock replyText = this.replyText;
        Visibility visibility1;
        ((UIElement) this.replyTextBorder).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 0));
        Visibility visibility2 = visibility1;
        ((UIElement) replyText).put_Visibility(visibility2);
      }
    }

    private async void showOrHide(bool forceTransition = false, bool scrollToBottomIfOpen = false)
    {
      if (this.RepliesShown)
      {
        this.replyText.put_Text("- " + App.Strings["videos.comments.hidereplies", "hide replies"]);
        if (this.Comment != null)
        {
          if (this.Comment.ReplyCount > this.Comment.Replies.Count && this.Comment.Replies.Count < 10)
          {
            ((UIElement) this.replyTextBorder).put_IsHitTestVisible(false);
            Ani.Begin((DependencyObject) this.replyTextBorder, "Opacity", 0.0, 0.2);
            ((UIElement) this.progress).put_Visibility((Visibility) 0);
            this.progress.put_IsActive(true);
            try
            {
              Comment[] feed = await CommentClient.GetRepliesClient(this.Comment.ID, 100).GetFeed(0);
              foreach (Comment comment in feed)
                comment.CommentThreadId = this.Comment.CommentThreadId;
              if (feed.Length != 0)
              {
                this.Comment.Replies.Clear();
                ((IList<Comment>) this.Comment.Replies).Add<Comment>((IList<Comment>) Enumerable.ToArray<Comment>(Enumerable.Reverse<Comment>((IEnumerable<Comment>) feed)));
              }
            }
            catch
            {
            }
            ((UIElement) this.progress).put_Visibility((Visibility) 1);
            this.progress.put_IsActive(false);
            ((UIElement) this.replyTextBorder).put_IsHitTestVisible(true);
            Ani.Begin((DependencyObject) this.replyTextBorder, "Opacity", 1.0, 0.2);
          }
          this.replyList.put_ItemsSource((object) this.Comment.Replies.ToArray());
        }
        await ((FrameworkElement) this).WaitForLayoutUpdateAsync();
        if (this.scroll == null)
          this.scroll = Helper.FindParentFromTree<ScrollViewer>((FrameworkElement) this, 1000);
        if (this.scroll != null)
        {
          Rect bounds = ((FrameworkElement) this).GetBounds((UIElement) this.scroll);
          if (bounds.Y > 0.0)
          {
            double num1 = 0.0;
            bool flag = false;
            if (bounds.Height > this.scroll.ViewportHeight)
            {
              num1 = 9.5;
              flag = true;
            }
            else if (bounds.Bottom > this.scroll.ViewportHeight)
            {
              num1 = this.scroll.ViewportHeight - bounds.Height - 57.0;
              flag = true;
            }
            if (num1 < 9.5)
              num1 = 9.5;
            double num2 = this.scroll.VerticalOffset + bounds.Y - num1;
            if (flag && num2 > this.scroll.VerticalOffset)
              this.scroll.ChangeView(new double?(), new double?(num2), new float?(), false);
          }
        }
      }
      else
      {
        string str = "+ " + App.Strings["videos.comments.showreplies", "show all replies"] + " (" + (object) this.Comment.ReplyCount + ")";
        if (this.replyText.Text != str)
          this.replyText.put_Text(str);
        if (this.Comment != null && this.Comment.Replies.Count > 0)
        {
          IEnumerable<Comment> comments = Enumerable.Where<Comment>((IEnumerable<Comment>) this.Comment.Replies, (Func<Comment, bool>) (c => c == Enumerable.Last<Comment>((IEnumerable<Comment>) this.Comment.Replies)));
          bool flag = true;
          if (this.replyList.ItemsSource != null)
          {
            Comment[] array1 = Enumerable.ToArray<Comment>((IEnumerable<Comment>) this.replyList.ItemsSource);
            if (array1.Length == 1)
            {
              Comment[] array2 = Enumerable.ToArray<Comment>(comments);
              if (array1[0] == array2[0])
                flag = false;
            }
          }
          if (flag)
            this.replyList.put_ItemsSource((object) comments);
        }
      }
      if (this.lastShowHide != this.RepliesShown | forceTransition)
      {
        double num = this.RepliesShown ? -50.0 : 50.0;
        ((UIElement) this.replyList).put_Opacity(0.0);
        this.repliesTrans.put_Y(num);
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.replyList, "Opacity", 1.0, 0.2), (Timeline) Ani.Begin((DependencyObject) this.repliesTrans, "Y", 0.0, 0.35, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
        if (this.RepliesShown & scrollToBottomIfOpen)
        {
          if (this.scroll == null)
            this.scroll = Helper.FindParentFromTree<ScrollViewer>((FrameworkElement) this, 1000);
          if (this.scroll != null)
          {
            Rect bounds = ((FrameworkElement) this).GetBounds((UIElement) this.scroll);
            if (bounds.Bottom > this.scroll.ViewportHeight)
              this.scroll.ChangeView(new double?(), new double?(this.scroll.VerticalOffset + (bounds.Bottom - this.scroll.ViewportHeight) + 19.0), new float?());
          }
        }
      }
      this.lastShowHide = this.RepliesShown;
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e) => this.RepliesShown = !this.RepliesShown;

    private void CommentControl_RepliedTo(object sender, Comment e)
    {
      if (this.Comment == null)
        return;
      this.Comment.Replies.Add(e);
      this.setVisibilities();
      this.showOrHide(true, true);
    }

    protected virtual Size ArrangeOverride(Size finalSize) => ((FrameworkElement) this).ArrangeOverride(finalSize);

    protected virtual Size MeasureOverride(Size availableSize) => ((FrameworkElement) this).MeasureOverride(availableSize);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///CommentThread.xaml"), (ComponentResourceLocation) 0);
      this.showRepliesStyle = (Style) ((FrameworkElement) this).FindName("showRepliesStyle");
      this.repliesTrans = (TranslateTransform) ((FrameworkElement) this).FindName("repliesTrans");
      this.progress = (ProgressRing) ((FrameworkElement) this).FindName("progress");
      this.replyList = (ItemsControl) ((FrameworkElement) this).FindName("replyList");
      this.replyTextBorder = (Border) ((FrameworkElement) this).FindName("replyTextBorder");
      this.mainControl = (CommentControl) ((FrameworkElement) this).FindName("mainControl");
      this.replyText = (TextBlock) ((FrameworkElement) this).FindName("replyText");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((CommentControl) target).RepliedTo += new EventHandler<Comment>(this.CommentControl_RepliedTo);
          break;
        case 2:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.Grid_Tapped));
          break;
        case 3:
          ((CommentControl) target).RepliedTo += new EventHandler<Comment>(this.CommentControl_RepliedTo);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
