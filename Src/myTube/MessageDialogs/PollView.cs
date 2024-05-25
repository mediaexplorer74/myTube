// myTube.MessageDialogs.PollView

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
  public sealed partial class PollView : UserControl
  {
    private PollDataClient client;
    private Task<int> userVotedTask;
    private bool viewingResults;
    private string lastId = "-1";

    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate itemTemplate;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate resultTemplate;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style listViewItem;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl viewResultsContent;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView choicesView;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl voteButton;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock viewResultsText;
    
    public PollView()
    {
      //this.InitializeComponent();
      this.client = new PollDataClient();
      ((FrameworkElement)this).DataContextChanged += PollView_DataContextChanged;
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
          ((Selector) this.choicesView).SelectedIndex = userVotedTask;
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
        ((UIElement) this.viewResultsContent).IsHitTestVisible = false;
      }
      else
        this.viewResultsText.Text = "view choices";
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      if (this.viewingResults)
      {
        tcs.SetResult(true);
        return tcs.Task;
      }
      this.viewingResults = true;
      ((UIElement) this.voteButton).IsHitTestVisible = (false);
      ((UIElement) this.choicesView).IsHitTestVisible = (false);
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 0.5, 0.3);
      int transDist = 100;
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) this.choicesView).RenderTransform = (Transform) trans;
      if (setSelectedItemNull)
        ((Selector) this.choicesView).SelectedItem = (object) null;
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni(
          (DependencyObject) this.choicesView, "Opacity", 0.0, 0.15),
          (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", (double) transDist, 0.15, 
          (EasingFunctionBase) Ani.Ease((EasingMode) 1, 4.0)));

           
            storyboard.Completed += async (sender, args) =>
            {
                trans.X = (double)(-transDist);
                ((Control)this.choicesView).IsEnabled = false;
                ((ItemsControl)this.choicesView).ItemTemplate = this.resultTemplate;
                tcs.SetResult(true);
                await Task.Delay(400);
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.choicesView, "Opacity", 1.0, 0.3), 
                    (Timeline)Ani.DoubleAni((DependencyObject)trans, "X", 0.0, 0.3,
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 4.0)));
            };


            /*
                    WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>,
                      EventRegistrationToken>(((Timeline) storyboard).add_Completed), 
                      new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed),
                      (EventHandler<object>) (async (_param1, _param2) =>
                  {
                    trans.X = ((double) -transDist);
                    ((Control) this.choicesView).IsEnabled = (false);
                    ((ItemsControl) this.choicesView).ItemTemplate = (this.resultTemplate);
                    tcs.SetResult(true);
                    await Task.Delay(400);
                    Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.choicesView, "Opacity", 1.0, 0.3), 
                        (Timeline) Ani.DoubleAni((DependencyObject) trans, "X", 0.0, 0.3,
                        (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)));
                  }));
            */

            ((Timeline)storyboard).Completed -= async (sender, args) =>
            {
                trans.X = ((double)-transDist);
                ((Control)this.choicesView).IsEnabled = (false);
                ((ItemsControl)this.choicesView).ItemTemplate = (this.resultTemplate);
                tcs.SetResult(true);
                await Task.Delay(400);
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.choicesView, "Opacity", 1.0, 0.3),
                    (Timeline)Ani.DoubleAni((DependencyObject)trans, "X", 0.0, 0.3,
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 4.0)));
            };

            return tcs.Task;
    }

        private async Task<bool> GoToVoting()
        {
            Ani.Begin((DependencyObject)this.viewResultsContent, "Opacity", 1.0, 0.2);
            ((UIElement)this.viewResultsContent).IsHitTestVisible = true;
            this.viewResultsText.Text = "view results";
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            if (!this.viewingResults)
            {
                tcs.SetResult(true);
                return await tcs.Task;
            }

            this.viewingResults = false;
            int transDist = 100;
            TranslateTransform trans = new TranslateTransform();
            ((UIElement)this.choicesView).RenderTransform = (Transform)trans;

            Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.choicesView, "Opacity", 0.01, 0.15), (Timeline)Ani.DoubleAni((DependencyObject)trans, "X", (double)transDist, 0.15, (EasingFunctionBase)Ani.Ease((EasingMode)1, 4.0)));

            storyboard.Completed += async (sender, args) =>
            {
                trans.X = (double)(-transDist);
                ((Control)this.choicesView).IsEnabled = true;

                if (this.itemTemplate == null 
                && ((IDictionary<object, object>)((FrameworkElement)this).Resources)
                .ContainsKey((object)"itemTemplate"))
                {
                    this.itemTemplate = ((IDictionary<object, object>)
                    ((FrameworkElement)this).Resources)[(object)"itemTemplate"] as DataTemplate;
                }

                ((ItemsControl)this.choicesView).ItemTemplate = (this.itemTemplate);
                tcs.SetResult(true);
                ((UIElement)this.voteButton).IsHitTestVisible = true;
                ((UIElement)this.choicesView).IsHitTestVisible = true;
                Ani.Begin((DependencyObject)this.voteButton, "Opacity", 1.0, 0.3);
                await Task.Delay(400);
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.choicesView, "Opacity", 1.0, 0.3), 
                    (Timeline)Ani.DoubleAni((DependencyObject)trans, "X", 0.0, 0.3,
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 4.0)));
            };

            return await tcs.Task;
        }

        private async void voteButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      bool vote = true;
      ((UIElement) this.voteButton).IsHitTestVisible = false;
      ((UIElement) this.choicesView).IsHitTestVisible = false;
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 0.5, 0.3);
      if (((Selector) this.choicesView).SelectedIndex == -1)
      {
        new MessageDialog("You must select a choice before you vote.", "Select a choice").ShowAsync();
        vote = false;
      }
      await App.CheckRykenUser();
      if (Settings.RykenUserID == null)
      {
        IUICommand iuiCommand = await new MessageDialog(
            "We're unable to authenticate you use a Ryken Apps user.", "Oops").ShowAsync();
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
            if (await this.client.Vote(data.Id, Settings.RykenUserID,
                ((Selector) this.choicesView).SelectedIndex) != -1)
            {
              int ind = ((Selector) this.choicesView).SelectedIndex;
              int num1 = await this.GoToResults() ? 1 : 0;
              ObservableCollection<int> pollVotesList = data.PollVotesList;
              int selectedIndex = ((Selector) this.choicesView).SelectedIndex;
              int num2 = ((Collection<int>) pollVotesList)[selectedIndex];
              ((Collection<int>) pollVotesList)[selectedIndex] = num2 + 1;
              ((Selector)this.choicesView).SelectedIndex = ind;
              data.NotifyChanged("SubPollData");
              returnButtons = false;
            }
          }
          else
          {
            int num = await this.GoToResults() ? 1 : 0;
            ((Selector) this.choicesView).SelectedIndex = userVoted;
            returnButtons = false;
          }
        }
        catch
        {
        }
      }
      if (!returnButtons)
        return;
      ((UIElement) this.choicesView).IsHitTestVisible =true;
      ((UIElement) this.voteButton).IsHitTestVisible=true;
      Ani.Begin((DependencyObject) this.voteButton, "Opacity", 1.0, 0.3);
    }

    private void viewResultsContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.viewingResults)
        this.GoToVoting();
      else
        this.GoToResults(false, true);
    }

  
  }
}
