// Decompiled with JetBrains decompiler
// Type: myTube.CommentsList
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Debug;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
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

namespace myTube
{
  public sealed class CommentsList : UserControl, IComponentConnector
  {
    public static DependencyProperty CommentsProperty = DependencyProperty.Register(nameof (Comments), typeof (ObservableCollection<Comment>), typeof (CommentsList), new PropertyMetadata((object) new ObservableCollection<Comment>()));
    public static DependencyProperty ClientProperty = DependencyProperty.Register(nameof (Client), typeof (CommentClient), typeof (CommentsList), new PropertyMetadata((object) null, new PropertyChangedCallback(CommentsList.OnClientChanged)));
    private string lastId = "";
    private int page;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate comentsTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl sortControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noComments;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressRing progressRing;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox commentBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl postCommentButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl itemsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run sortRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnClientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CommentsList commentsList = d as CommentsList;
      if (e.NewValue == null)
        return;
      commentsList.setOrderText();
      commentsList.Clear();
      commentsList.Load();
    }

    private YouTubeEntry Entry => ((FrameworkElement) this).DataContext as YouTubeEntry;

    public ObservableCollection<Comment> Comments => (ObservableCollection<Comment>) ((DependencyObject) this).GetValue(CommentsList.CommentsProperty);

    public CommentClient Client
    {
      get => (CommentClient) ((DependencyObject) this).GetValue(CommentsList.ClientProperty);
      set => ((DependencyObject) this).SetValue(CommentsList.ClientProperty, (object) value);
    }

    public CommentsList()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(CommentsList_DataContextChanged)));
      this.itemsControl.put_ItemsSource((object) this.Comments);
      TextBox commentBox1 = this.commentBox;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) commentBox1).add_GotFocus), new Action<EventRegistrationToken>(((UIElement) commentBox1).remove_GotFocus), new RoutedEventHandler(this.CommentBox_GotFocus));
      TextBox commentBox2 = this.commentBox;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) commentBox2).add_LostFocus), new Action<EventRegistrationToken>(((UIElement) commentBox2).remove_LostFocus), new RoutedEventHandler(this.CommentBox_LostFocus));
    }

    private void CommentBox_LostFocus(object sender, RoutedEventArgs e)
    {
    }

    private void CommentBox_GotFocus(object sender, RoutedEventArgs e)
    {
    }

    private void CommentsList_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Entry == null)
        return;
      int num = this.Entry.ID == this.lastId ? 1 : 0;
    }

    public async Task Clear()
    {
      this.page = 0;
      ((Collection<Comment>) this.Comments).Clear();
    }

    public async Task Load()
    {
      if (this.Client == null)
        return;
      ((UIElement) this.noComments).put_Visibility((Visibility) 1);
      this.progressRing.put_IsActive(true);
      try
      {
        foreach (Comment comment in await this.Client.GetFeed(this.page))
          ((Collection<Comment>) this.Comments).Add(comment);
        ++this.page;
      }
      catch
      {
      }
      this.progressRing.put_IsActive(false);
      if (((Collection<Comment>) this.Comments).Count != 0)
        return;
      ((UIElement) this.noComments).put_Visibility((Visibility) 0);
    }

    private async void sortControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      MenuFlyout menuFlyout = new MenuFlyout();
      MenuFlyoutItem menuFlyoutItem1 = new MenuFlyoutItem();
      menuFlyoutItem1.put_Text(App.Strings["search.options.sortby.relevance", "relevance"].ToLower());
      ((Control) menuFlyoutItem1).put_IsEnabled(Settings.CommentsOrder != 0);
      MenuFlyoutItem menuFlyoutItem2 = menuFlyoutItem1;
      MenuFlyoutItem menuFlyoutItem3 = new MenuFlyoutItem();
      menuFlyoutItem3.put_Text(App.Strings["search.options.sortby.recent", "most recent"].ToLower());
      ((Control) menuFlyoutItem3).put_IsEnabled(Settings.CommentsOrder != YouTubeOrder.Published);
      MenuFlyoutItem menuFlyoutItem4 = menuFlyoutItem3;
      menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem2);
      menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem4);
      MenuFlyoutItem menuFlyoutItem5 = menuFlyoutItem2;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem5.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem5.remove_Click), (RoutedEventHandler) ((_param1, _param2) => this.changeOrder(YouTubeOrder.Relevance)));
      MenuFlyoutItem menuFlyoutItem6 = menuFlyoutItem4;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem6.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem6.remove_Click), (RoutedEventHandler) ((_param1, _param2) => this.changeOrder(YouTubeOrder.Published)));
      ((FlyoutBase) menuFlyout).ShowAt((FrameworkElement) this.sortControl);
    }

    private void setOrderText()
    {
      if (Settings.CommentsOrder == YouTubeOrder.Relevance)
        this.sortRun.put_Text(App.Strings["search.options.sortby.relevance", "relevance"].ToLower());
      else
        this.sortRun.put_Text(App.Strings["search.options.sortby.recent", "most recent"].ToLower());
    }

    private void changeOrder(YouTubeOrder order)
    {
      if (Settings.CommentsOrder == order)
        return;
      Settings.CommentsOrder = order;
      this.setOrderText();
      if (this.Client == null)
        return;
      this.Client.Order = order;
      this.Clear();
      this.Load();
    }

    private async void postCommentButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      try
      {
        await App.CheckSignIn(45.0);
      }
      catch
      {
      }
      if (!YouTube.IsSignedIn)
        DefaultPage.Current.OpenBrowser();
      if (!SharedSettings.CurrentAccount.Scope.Contains("https://www.googleapis.com/auth/youtube.force-ssl"))
        DefaultPage.Current.OpenBrowser("You need to re-sign into your account to post comments. This only has to be done once for this account.");
      else if (string.IsNullOrWhiteSpace(this.commentBox.Text))
      {
        ((Control) this.commentBox).Focus((FocusState) 3);
      }
      else
      {
        string text = this.commentBox.Text.Trim();
        if (this.Client != null)
        {
          ((Control) this.commentBox).put_IsEnabled(false);
          ((UIElement) this.postCommentButton).put_IsHitTestVisible(false);
          Ani.Begin((DependencyObject) this.postCommentButton, "Opacity", 0.5, 0.2);
          try
          {
            Comment comment = await CommentClient.Post(this.Client.ParentId, text);
            comment.Content = text;
            comment.Time = DateTime.Now;
            ((Collection<Comment>) this.Comments).Insert(0, comment);
            ((UIElement) this.noComments).put_Visibility((Visibility) 1);
            this.commentBox.put_Text("");
          }
          catch
          {
          }
        }
        ((Control) this.commentBox).put_IsEnabled(true);
        ((UIElement) this.postCommentButton).put_IsHitTestVisible(true);
        Ani.Begin((DependencyObject) this.postCommentButton, "Opacity", 1.0, 0.5);
      }
    }

    private void sortControl_Holding(object sender, HoldingRoutedEventArgs e)
    {
      if (Settings.UserMode < UserMode.Beta)
        return;
      DefaultPage.Current.Frame.Navigate(typeof (CommentListTestPage), (object) this.Client);
    }

    protected virtual void OnKeyDown(KeyRoutedEventArgs e) => ((Control) this).OnKeyDown(e);

    protected virtual void OnKeyUp(KeyRoutedEventArgs e) => ((Control) this).OnKeyUp(e);

    private void commentBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///CommentsList.xaml"), (ComponentResourceLocation) 0);
      this.comentsTemplate = (DataTemplate) ((FrameworkElement) this).FindName("comentsTemplate");
      this.sortControl = (ContentControl) ((FrameworkElement) this).FindName("sortControl");
      this.noComments = (TextBlock) ((FrameworkElement) this).FindName("noComments");
      this.progressRing = (ProgressRing) ((FrameworkElement) this).FindName("progressRing");
      this.commentBox = (TextBox) ((FrameworkElement) this).FindName("commentBox");
      this.postCommentButton = (ContentControl) ((FrameworkElement) this).FindName("postCommentButton");
      this.itemsControl = (ItemsControl) ((FrameworkElement) this).FindName("itemsControl");
      this.sortRun = (Run) ((FrameworkElement) this).FindName("sortRun");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.sortControl_Tapped));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement2.add_Holding), new Action<EventRegistrationToken>(uiElement2.remove_Holding), new HoldingEventHandler(this.sortControl_Holding));
          break;
        case 2:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement3.add_KeyDown), new Action<EventRegistrationToken>(uiElement3.remove_KeyDown), new KeyEventHandler(this.commentBox_KeyDown));
          break;
        case 3:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.postCommentButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
