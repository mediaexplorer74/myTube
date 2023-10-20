// Decompiled with JetBrains decompiler
// Type: myTube.MessageDialogs.PollView
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace myTube.MessageDialogs
{
  public sealed class PollView : UserControl, IComponentConnector
  {
    private PollDataClient client;
    private Task<int> userVotedTask;
    private bool viewingResults;
    private string lastId = "-1";
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate itemTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate resultTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style listViewItem;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl viewResultsContent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView choicesView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl voteButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock viewResultsText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public PollView()
    {
      this.InitializeComponent();
      this.client = new PollDataClient();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(PollView_DataContextChanged)));
    }

    private async void PollView_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is PollData dataContext) || dataContext.Id == this.lastId)
        return;
      if (Settings.RykenUserID == null)
      {
        this.GoToResults();
      }
      else
      {
        try
        {
          this.userVotedTask = this.client.UserVotedOnPoll(dataContext.Id, Settings.RykenUserID);
          int userVotedTask = await this.userVotedTask;
          if (userVotedTask == -1)
            return;
          ((Selector) this.choicesView).put_SelectedIndex(userVotedTask);
          this.GoToResults();
        }
        catch
        {
        }
      }
    }

    private Task<bool> GoToResults(bool hideViewResultsButton = true, bool setSelectedItemNull = false)
    {
      if (hideViewResultsButton)
      {
        Ani.Begin((DependencyObject) this.viewResultsContent, "Opacity", 0.0, 0.2);
        ((UIElement) this.viewResultsContent).put_IsHitTestVisible(false);
      }
      else
        this.viewResultsText.put_Text("view choices");
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      if (this.viewingResults)
      {
        tcs.SetResult(true);
        return tcs.Task;
      }
      this.viewingResults = true;
      ((UIElement) this.voteButton).put_IsHitTestVisible(false);
      ((UIElement) this.choicesView).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 0.5, 0.3);
      int transDist = 100;
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) this.choicesView).put_RenderTransform((Transform) trans);
      if (setSelectedItemNull)
        ((Selector) this.choicesView).put_SelectedItem((object) null);
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.choicesView, "Opacity", 0.0, 0.15), (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", (double) transDist, 0.15, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 4.0)));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) (async (_param1, _param2) =>
      {
        trans.put_X((double) -transDist);
        ((Control) this.choicesView).put_IsEnabled(false);
        ((ItemsControl) this.choicesView).put_ItemTemplate(this.resultTemplate);
        tcs.SetResult(true);
        await Task.Delay(400);
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.choicesView, "Opacity", 1.0, 0.3), (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", 0.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)));
      }));
      return tcs.Task;
    }

    private Task<bool> GoToVoting()
    {
      Ani.Begin((DependencyObject) this.viewResultsContent, "Opacity", 1.0, 0.2);
      ((UIElement) this.viewResultsContent).put_IsHitTestVisible(true);
      this.viewResultsText.put_Text("view results");
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      if (!this.viewingResults)
      {
        tcs.SetResult(true);
        return tcs.Task;
      }
      this.viewingResults = false;
      int transDist = 100;
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) this.choicesView).put_RenderTransform((Transform) trans);
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.choicesView, "Opacity", 0.01, 0.15), (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", (double) transDist, 0.15, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 4.0)));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) (async (_param1, _param2) =>
      {
        trans.put_X((double) -transDist);
        ((Control) this.choicesView).put_IsEnabled(true);
        if (this.itemTemplate == null && ((IDictionary<object, object>) ((FrameworkElement) this).Resources).ContainsKey((object) "itemTemplate"))
          this.itemTemplate = ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "itemTemplate"] as DataTemplate;
        ((ItemsControl) this.choicesView).put_ItemTemplate(this.itemTemplate);
        tcs.SetResult(true);
        ((UIElement) this.voteButton).put_IsHitTestVisible(true);
        ((UIElement) this.choicesView).put_IsHitTestVisible(true);
        Ani.Begin((DependencyObject) this.voteButton, "Opacity", 1.0, 0.3);
        await Task.Delay(400);
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.choicesView, "Opacity", 1.0, 0.3), (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", 0.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)));
      }));
      return tcs.Task;
    }

    private async void voteButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      bool vote = true;
      ((UIElement) this.voteButton).put_IsHitTestVisible(false);
      ((UIElement) this.choicesView).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 0.5, 0.3);
      if (((Selector) this.choicesView).SelectedIndex == -1)
      {
        new MessageDialog("You must select a choice before you vote.", "Select a choice").ShowAsync();
        vote = false;
      }
      await App.CheckRykenUser();
      if (Settings.RykenUserID == null)
      {
        IUICommand iuiCommand = await new MessageDialog("We're unable to authenticate you use a Ryken Apps user.", "Oops").ShowAsync();
        vote = false;
      }
      PollData data = ((FrameworkElement) this).DataContext as PollData;
      bool returnButtons = true;
      if (data != null && vote)
      {
        try
        {
          int userVoted = -1;
          try
          {
            userVoted = await this.userVotedTask;
          }
          catch
          {
          }
          if (userVoted == -1)
          {
            if (await this.client.Vote(data.Id, Settings.RykenUserID, ((Selector) this.choicesView).SelectedIndex) != -1)
            {
              int ind = ((Selector) this.choicesView).SelectedIndex;
              int num1 = await this.GoToResults() ? 1 : 0;
              ObservableCollection<int> pollVotesList = data.PollVotesList;
              int selectedIndex = ((Selector) this.choicesView).SelectedIndex;
              int num2 = ((Collection<int>) pollVotesList)[selectedIndex];
              ((Collection<int>) pollVotesList)[selectedIndex] = num2 + 1;
              ((Selector) this.choicesView).put_SelectedIndex(ind);
              data.NotifyChanged("SubPollData");
              returnButtons = false;
            }
          }
          else
          {
            int num = await this.GoToResults() ? 1 : 0;
            ((Selector) this.choicesView).put_SelectedIndex(userVoted);
            returnButtons = false;
          }
        }
        catch
        {
        }
      }
      if (!returnButtons)
        return;
      ((UIElement) this.choicesView).put_IsHitTestVisible(true);
      ((UIElement) this.voteButton).put_IsHitTestVisible(true);
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 1.0, 0.3);
    }

    private void viewResultsContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.viewingResults)
        this.GoToVoting();
      else
        this.GoToResults(false, true);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///MessageDialogs/PollView.xaml"), (ComponentResourceLocation) 0);
      this.itemTemplate = (DataTemplate) ((FrameworkElement) this).FindName("itemTemplate");
      this.resultTemplate = (DataTemplate) ((FrameworkElement) this).FindName("resultTemplate");
      this.listViewItem = (Style) ((FrameworkElement) this).FindName("listViewItem");
      this.viewResultsContent = (ContentControl) ((FrameworkElement) this).FindName("viewResultsContent");
      this.choicesView = (ListView) ((FrameworkElement) this).FindName("choicesView");
      this.voteButton = (ContentControl) ((FrameworkElement) this).FindName("voteButton");
      this.viewResultsText = (TextBlock) ((FrameworkElement) this).FindName("viewResultsText");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.viewResultsContent_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.voteButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
