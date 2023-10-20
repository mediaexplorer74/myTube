// Decompiled with JetBrains decompiler
// Type: myTube.MessageDialogs.CommentsView
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace myTube.MessageDialogs
{
  public sealed class CommentsView : UserControl, IComponentConnector
  {
    private const int MaxCharacters = 200;
    private myTube.Cloud.Clients.CommentClient client;
    private int page;
    private CompositeTransform trans;
    private InputPane inputPane;
    private bool started;
    private string lastMessageId = "-1";
    private bool load = true;
    private bool loading;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl closeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noCommentsText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer commentsScroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock textCounter;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox commentTextBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl postControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl commentsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public Message Message => ((FrameworkElement) this).DataContext is Message ? (Message) ((FrameworkElement) this).DataContext : (Message) null;

    public event EventHandler Closing;

    public event EventHandler Closed;

    public CommentsView()
    {
      this.client = new myTube.Cloud.Clients.CommentClient();
      this.InitializeComponent();
      this.trans = new CompositeTransform();
      ((UIElement) this).put_RenderTransformOrigin(new Point(0.5, 0.5));
      ((UIElement) this).put_RenderTransform((Transform) this.trans);
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.CommentsView_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.CommentsView_Unloaded));
      this.textCounter.put_Text("0 / " + (object) 200);
    }

    private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
    {
      e.put_Handled(true);
      this.Close();
    }

    private void UnsubscribeFromInputPanel()
    {
      if (this.inputPane == null)
        return;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Action<EventRegistrationToken>(this.inputPane.remove_Showing), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(inputPane_Showing)));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Action<EventRegistrationToken>(this.inputPane.remove_Hiding), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(inputPane_Hiding)));
      this.inputPane = (InputPane) null;
    }

    private void SubscribeToInputPane()
    {
      this.UnsubscribeFromInputPanel();
      this.inputPane = InputPane.GetForCurrentView();
      InputPane inputPane1 = this.inputPane;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(inputPane1.add_Showing), new Action<EventRegistrationToken>(inputPane1.remove_Showing), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(inputPane_Showing)));
      InputPane inputPane2 = this.inputPane;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(inputPane2.add_Hiding), new Action<EventRegistrationToken>(inputPane2.remove_Hiding), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) this, __methodptr(inputPane_Hiding)));
    }

    private void inputPane_Hiding(InputPane sender, InputPaneVisibilityEventArgs args) => Ani.Begin((DependencyObject) this.trans, "TranslateY", 0.0, 0.5, 5.0);

    private void inputPane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      if (((Control) this.commentTextBox).FocusState == null)
        return;
      double num = this.inputPane.OccludedRect.Top - ((FrameworkElement) this.commentTextBox).GetBounds((UIElement) DefaultPage.Current).Bottom;
      if (num >= 0.0)
        return;
      Ani.Begin((DependencyObject) this.trans, "TranslateY", num - 19.0, 0.5, 5.0);
    }

    private void CommentsView_Unloaded(object sender, RoutedEventArgs e)
    {
      DefaultPage.Current.BackPressed -= new EventHandler<BackPressedEventArgs>(this.HardwareButtons_BackPressed);
      this.UnsubscribeFromInputPanel();
    }

    private void CommentsView_Loaded(object sender, RoutedEventArgs e)
    {
      DefaultPage.Current.BackPressed += new EventHandler<BackPressedEventArgs>(this.HardwareButtons_BackPressed);
      this.SubscribeToInputPane();
      this.Open();
    }

    private void Open()
    {
      ((UIElement) this).put_Opacity(0.0);
      ((UIElement) this.commentsControl).put_Visibility((Visibility) 1);
      ((UIElement) this.commentsControl).put_Opacity(0.0);
      this.trans.put_ScaleY(0.3);
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.trans, "ScaleY", 1.0, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)), (Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 1.0, 0.1));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        if (!this.started)
        {
          if (((FrameworkElement) this).DataContext != null)
            this.CommentsView_DataContextChanged((FrameworkElement) null, (DataContextChangedEventArgs) null);
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(CommentsView_DataContextChanged)));
        }
        this.started = true;
        ((UIElement) this.commentsControl).put_Visibility((Visibility) 0);
        Ani.Begin((DependencyObject) this.commentsControl, "Opacity", 1.0, 0.2);
      }));
    }

    private void Close()
    {
      if (this.Closing != null)
        this.Closing((object) this, (EventArgs) null);
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.trans, "ScaleY", 0.3, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), (Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 0.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.commentsControl, "Opacity", 0.0, 0.1));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        if (this.Closed == null)
          return;
        this.Closed((object) this, (EventArgs) null);
      }));
    }

    private void CommentsView_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Message == null || this.Message.Id == this.lastMessageId)
        return;
      this.lastMessageId = this.Message.Id;
      this.Clear();
      this.LoadComments();
    }

    private void Clear()
    {
      ((ICollection<object>) this.commentsControl.Items).Clear();
      this.page = 0;
      this.load = true;
    }

    private async void LoadComments()
    {
      if (!this.load)
        return;
      this.loading = true;
      Task<myTube.Cloud.Comment[]> comments = this.client.GetComments(this.Message.Id, 20, this.page);
      ++this.page;
      myTube.Cloud.Comment[] commentArray = await comments;
      foreach (object obj in commentArray)
        ((ICollection<object>) this.commentsControl.Items).Add(obj);
      if (((ICollection<object>) this.commentsControl.Items).Count > 0)
        ((UIElement) this.noCommentsText).put_Visibility((Visibility) 1);
      else
        ((UIElement) this.noCommentsText).put_Visibility((Visibility) 0);
      this.loading = false;
      if (commentArray.Length != 0)
        return;
      this.load = false;
    }

    private async void postControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.commentTextBox.Text))
      {
        await Task.Delay(300);
        ((Control) this.commentTextBox).Focus((FocusState) 3);
      }
      else
      {
        ((Control) this.commentTextBox).put_IsEnabled(false);
        Ani.Begin((DependencyObject) this.postControl, "Opacity", 0.5, 0.2);
        ((UIElement) this.postControl).put_IsHitTestVisible(false);
        myTube.Cloud.Comment comment1 = new myTube.Cloud.Comment()
        {
          Content = this.commentTextBox.Text,
          UserID = Settings.RykenUserID,
          MessageID = this.Message.Id
        };
        if (YouTube.IsSignedIn)
          comment1.UserName = YouTube.UserInfo?.UserDisplayName;
        try
        {
          myTube.Cloud.Comment comment2 = await this.client.PostComment(comment1);
          if (comment2 != null)
          {
            this.commentsScroll.ChangeView(new double?(), new double?(0.0), new float?());
            ((IList<object>) this.commentsControl.Items).Insert(0, (object) comment2);
          }
        }
        catch
        {
        }
        ((Control) this.commentTextBox).put_IsEnabled(true);
        this.commentTextBox.put_Text("");
        Ani.Begin((DependencyObject) this.postControl, "Opacity", 1.0, 0.2);
        ((UIElement) this.postControl).put_IsHitTestVisible(true);
        ((UIElement) this.noCommentsText).put_Visibility((Visibility) 1);
      }
    }

    private void closeButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Close();

    private void commentsScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
      if (this.commentsScroll.ScrollableHeight - this.commentsScroll.VerticalOffset >= 600.0 || this.loading)
        return;
      this.LoadComments();
    }

    private void commentTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.commentTextBox.Text.Length > 200)
        this.commentTextBox.put_Text(this.commentTextBox.Text.Substring(0, 200));
      this.textCounter.put_Text(this.commentTextBox.Text.Length.ToString() + " / " + (object) 200);
    }

    private void commentTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (this.commentTextBox.Text.Length < 200 || e.Key == 8 || this.commentTextBox.SelectionLength != 0)
        return;
      e.put_Handled(true);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///MessageDialogs/CommentsView.xaml"), (ComponentResourceLocation) 0);
      this.closeButton = (ContentControl) ((FrameworkElement) this).FindName("closeButton");
      this.noCommentsText = (TextBlock) ((FrameworkElement) this).FindName("noCommentsText");
      this.commentsScroll = (ScrollViewer) ((FrameworkElement) this).FindName("commentsScroll");
      this.textCounter = (TextBlock) ((FrameworkElement) this).FindName("textCounter");
      this.commentTextBox = (TextBox) ((FrameworkElement) this).FindName("commentTextBox");
      this.postControl = (ContentControl) ((FrameworkElement) this).FindName("postControl");
      this.commentsControl = (ItemsControl) ((FrameworkElement) this).FindName("commentsControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.closeButton_Tapped));
          break;
        case 2:
          ScrollViewer scrollViewer = (ScrollViewer) target;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.commentsScroll_ViewChanged));
          break;
        case 3:
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.commentTextBox_TextChanged));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement2.add_KeyDown), new Action<EventRegistrationToken>(uiElement2.remove_KeyDown), new KeyEventHandler(this.commentTextBox_KeyDown));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.postControl_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
