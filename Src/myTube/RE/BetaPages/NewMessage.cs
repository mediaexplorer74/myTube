// Decompiled with JetBrains decompiler
// Type: myTube.BetaPages.NewMessage
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
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.BetaPages
{
  public sealed class NewMessage : Page, IComponentConnector
  {
    private MessageClient client;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PollData poll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar recentProgress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noRecentMessages;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl messagesList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl viewButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl button;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox title;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox body;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox minVersion;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox maxVersion;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox userMode;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl pollView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton pollAddButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl actionView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton actionAddButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public NewMessage()
    {
      this.client = new MessageClient();
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.NewMessage_Loaded));
      foreach (object obj in Enum.GetValues(typeof (UserMode)))
        ((ICollection<object>) ((ItemsControl) this.userMode).Items).Add(obj);
      ((Selector) this.userMode).put_SelectedIndex(0);
    }

    private void NewMessage_Loaded(object sender, RoutedEventArgs e) => this.LoadRecent();

    private async Task LoadRecent()
    {
      this.recentProgress.put_IsIndeterminate(true);
      ((UIElement) this.noRecentMessages).put_Visibility((Visibility) 1);
      try
      {
        Message[] recent = await this.client.GetRecent(50);
        if (recent.Length != 0)
        {
          this.messagesList.put_ItemsSource((object) Enumerable.ToList<Message>((IEnumerable<Message>) recent));
        }
        else
        {
          ((UIElement) this.noRecentMessages).put_Visibility((Visibility) 0);
          this.messagesList.put_ItemsSource((object) Enumerable.ToList<Message>((IEnumerable<Message>) recent));
        }
      }
      catch
      {
        ((UIElement) this.noRecentMessages).put_Visibility((Visibility) 0);
        this.messagesList.put_ItemsSource((object) new List<Message>());
      }
      this.recentProgress.put_IsIndeterminate(false);
    }

    private async void ContentControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Ani.Begin((DependencyObject) this.button, "Opacity", 0.5, 0.2);
      ((UIElement) this.button).put_IsHitTestVisible(false);
      if (string.IsNullOrWhiteSpace(this.title.Text))
      {
        await Task.Delay(400);
        ((Control) this.title).Focus((FocusState) 3);
        Ani.Begin((DependencyObject) this.button, "Opacity", 1.0, 0.2);
        ((UIElement) this.button).put_IsHitTestVisible(true);
      }
      else if (string.IsNullOrWhiteSpace(this.body.Text))
      {
        await Task.Delay(400);
        ((Control) this.body).Focus((FocusState) 3);
        Ani.Begin((DependencyObject) this.button, "Opacity", 1.0, 0.2);
        ((UIElement) this.button).put_IsHitTestVisible(true);
      }
      else
      {
        bool submit = false;
        if (((Collection<string>) this.poll.PollChoicesList).Count == 1)
        {
          IUICommand iuiCommand1 = await new MessageDialog("The poll must have at least two choices", "(Error) Add more poll choices").ShowAsync();
        }
        else
        {
          Message tm = this.CreateMessage();
          string s = "";
          s = s + "Title: " + tm.Title + "\n\nVersions [min, max]: " + tm.MinVersion + ", " + tm.MaxVersion + "\n\nUser mode: " + (object) tm.UserMode;
          await Task.Delay(100);
          if (((Collection<string>) this.poll.PollChoicesList).Count > 1)
          {
            s += "\n\nPoll choices: ";
            foreach (string pollChoices in (Collection<string>) this.poll.PollChoicesList)
              s = s + "\n" + pollChoices;
          }
          s = s + "\n\nBody: " + tm.Body;
          UICommand yes = new UICommand("ok");
          UICommand uiCommand = new UICommand("cancel");
          MessageDialog messageDialog = new MessageDialog(s, "Submit this message?");
          messageDialog.Commands.Add((IUICommand) yes);
          messageDialog.Commands.Add((IUICommand) uiCommand);
          if (await messageDialog.ShowAsync() == yes)
            submit = true;
          tm = (Message) null;
          s = (string) null;
          yes = (UICommand) null;
        }
        if (submit)
        {
          Message m = (Message) null;
          if (((Collection<string>) this.poll.PollChoicesList).Count > 0)
          {
            PollDataClient pollDataClient = new PollDataClient();
            try
            {
              m = this.CreateMessage((await pollDataClient.AddPoll(this.poll)).Id);
            }
            catch (Exception ex)
            {
              new MessageDialog(ex.ToString(), "Error submitting poll").ShowAsync();
            }
          }
          else
            m = this.CreateMessage();
          try
          {
            IUICommand iuiCommand2 = await new MessageDialog(await this.client.AddMessage(m), "Response from server").ShowAsync();
          }
          catch (Exception ex)
          {
            new MessageDialog(ex.ToString(), "Exception in uploading message").ShowAsync();
          }
          this.LoadRecent();
          m = (Message) null;
        }
        Ani.Begin((DependencyObject) this.button, "Opacity", 1.0, 0.2);
        ((UIElement) this.button).put_IsHitTestVisible(true);
      }
    }

    private Message CreateMessage(string pollId = "-1")
    {
      if (!Version.TryParse(this.minVersion.Text, out Version _) || !Version.TryParse(this.maxVersion.Text, out Version _))
        throw new InvalidOperationException("Not proper version numbers");
      Message message = new Message()
      {
        Title = this.title.Text,
        Body = this.body.Text,
        MinVersion = this.minVersion.Text,
        MaxVersion = this.maxVersion.Text,
        PollId = pollId,
        UserMode = (UserMode) ((Selector) this.userMode).SelectedItem
      };
      foreach (object obj in (IEnumerable<object>) this.actionView.Items)
      {
        if (obj is MessageAction)
          message.Actions.Add(obj as MessageAction);
      }
      return message;
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      await Task.Delay(200);
      ((UIElement) this.overCanvas).Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      ((UIElement) this.overCanvas).Measure(((UIElement) this.overCanvas).DesiredSize);
      ((UIElement) this.overCanvas).UpdateLayout();
      base.OnNavigatedTo(e);
    }

    private void viewButton_Tapped(object sender, TappedRoutedEventArgs e) => App.ShowMessage(this.CreateMessage());

    private void MessageThumb_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
      if (e.PointerDeviceType != 2)
        return;
      this.ShowRecentMenu(sender);
    }

    private void ShowRecentMenu(object sender)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      ((UIElement) frameworkElement).put_IsHitTestVisible(false);
      Message mess = frameworkElement.DataContext as Message;
      if (mess != null)
      {
        MenuFlyout menuFlyout = new MenuFlyout();
        MenuFlyoutItem menuFlyoutItem1 = new MenuFlyoutItem();
        menuFlyoutItem1.put_Text("delete");
        MenuFlyoutItem menuFlyoutItem2 = menuFlyoutItem1;
        MenuFlyoutItem menuFlyoutItem3 = menuFlyoutItem2;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem3.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem3.remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
        {
          int num = await this.client.Delete(mess.Id) ? 1 : 0;
          this.LoadRecent();
        }));
        menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem2);
        ((FlyoutBase) menuFlyout).ShowAt(frameworkElement);
      }
      ((UIElement) frameworkElement).put_IsHitTestVisible(true);
    }

    private void MessageThumb_Holding(object sender, HoldingRoutedEventArgs e)
    {
      if (e.HoldingState != null || e.PointerDeviceType != null)
        return;
      this.ShowRecentMenu(sender);
    }

    private void pollAddButton_Tapped(object sender, TappedRoutedEventArgs e) => ((Collection<string>) this.poll.PollChoicesList).Add("Choice " + (object) (((Collection<string>) this.poll.PollChoicesList).Count + 1));

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      FrameworkElement frameworkElement = sender as FrameworkElement;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement is TextBox textBox))
        return;
      int index = ((Collection<string>) this.poll.PollChoicesList).IndexOf((string) ((FrameworkElement) textBox).DataContext);
      if (index == -1 || !(((Collection<string>) this.poll.PollChoicesList)[index] != textBox.Text))
        return;
      ((Collection<string>) this.poll.PollChoicesList)[index] = textBox.Text;
    }

    private void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      ((Collection<string>) this.poll.PollChoicesList).Remove((string) frameworkElement.DataContext);
    }

    private async void actionAddButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      PopupMenu popupMenu = new PopupMenu();
      UICommand blank = new UICommand("New action");
      UICommand beta = new UICommand("Get the beta");
      UICommand update = new UICommand("Update");
      popupMenu.Commands.Add((IUICommand) blank);
      popupMenu.Commands.Add((IUICommand) beta);
      popupMenu.Commands.Add((IUICommand) update);
      IUICommand iuiCommand = await popupMenu.ShowAsync(e.GetPosition(Window.Current.Content));
      if (iuiCommand == blank)
        ((ICollection<object>) this.actionView.Items).Add((object) new MessageAction());
      else if (iuiCommand == beta)
      {
        ((ICollection<object>) this.actionView.Items).Add((object) new MessageAction()
        {
          Type = MessageActionType.Store,
          Data = "9wzdncrdt29j",
          Title = "get the beta"
        });
      }
      else
      {
        if (iuiCommand != update)
          return;
        ((ICollection<object>) this.actionView.Items).Add((object) new MessageAction()
        {
          Type = MessageActionType.Store,
          Title = "update now"
        });
      }
    }

    private void deleteAction_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !((ICollection<object>) this.actionView.Items).Contains(frameworkElement.DataContext))
        return;
      ((ICollection<object>) this.actionView.Items).Remove(frameworkElement.DataContext);
    }

    private void deleteAction_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void actionTypeBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || !(((FrameworkElement) comboBox).DataContext is MessageAction dataContext))
        return;
      Array values = Enum.GetValues(typeof (MessageActionType));
      ((ItemsControl) comboBox).put_ItemsSource((object) values);
      ((Selector) comboBox).put_SelectedItem((object) dataContext.Type);
    }

    private void actionTitleBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!(sender is TextBox textBox) || !(((FrameworkElement) textBox).DataContext is MessageAction dataContext))
        return;
      dataContext.Title = textBox.Text;
    }

    private void actionDataBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!(sender is TextBox textBox) || !(((FrameworkElement) textBox).DataContext is MessageAction dataContext))
        return;
      dataContext.Data = textBox.Text;
    }

    private void actionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(sender is ComboBox comboBox) || !(((FrameworkElement) comboBox).DataContext is MessageAction dataContext))
        return;
      dataContext.Type = (MessageActionType) ((Selector) comboBox).SelectedItem;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///BetaPages/NewMessage.xaml"), (ComponentResourceLocation) 0);
      this.poll = (PollData) ((FrameworkElement) this).FindName("poll");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.recentProgress = (ProgressBar) ((FrameworkElement) this).FindName("recentProgress");
      this.noRecentMessages = (TextBlock) ((FrameworkElement) this).FindName("noRecentMessages");
      this.messagesList = (ItemsControl) ((FrameworkElement) this).FindName("messagesList");
      this.viewButton = (ContentControl) ((FrameworkElement) this).FindName("viewButton");
      this.button = (ContentControl) ((FrameworkElement) this).FindName("button");
      this.title = (TextBox) ((FrameworkElement) this).FindName("title");
      this.body = (TextBox) ((FrameworkElement) this).FindName("body");
      this.minVersion = (TextBox) ((FrameworkElement) this).FindName("minVersion");
      this.maxVersion = (TextBox) ((FrameworkElement) this).FindName("maxVersion");
      this.userMode = (ComboBox) ((FrameworkElement) this).FindName("userMode");
      this.pollView = (ItemsControl) ((FrameworkElement) this).FindName("pollView");
      this.pollAddButton = (IconTextButton) ((FrameworkElement) this).FindName("pollAddButton");
      this.actionView = (ItemsControl) ((FrameworkElement) this).FindName("actionView");
      this.actionAddButton = (IconTextButton) ((FrameworkElement) this).FindName("actionAddButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement.add_Loaded), new Action<EventRegistrationToken>(frameworkElement.remove_Loaded), new RoutedEventHandler(this.actionTypeBox_Loaded));
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.actionTypeBox_SelectionChanged));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement1.add_LostFocus), new Action<EventRegistrationToken>(uiElement1.remove_LostFocus), new RoutedEventHandler(this.actionTitleBox_LostFocus));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement2.add_LostFocus), new Action<EventRegistrationToken>(uiElement2.remove_LostFocus), new RoutedEventHandler(this.actionDataBox_LostFocus));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.deleteAction_Tapped));
          break;
        case 5:
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.TextBox_TextChanged));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement4.add_GotFocus), new Action<EventRegistrationToken>(uiElement4.remove_GotFocus), new RoutedEventHandler(this.TextBox_GotFocus));
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement5.add_LostFocus), new Action<EventRegistrationToken>(uiElement5.remove_LostFocus), new RoutedEventHandler(this.TextBox_LostFocus));
          break;
        case 6:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.IconTextButton_Tapped));
          break;
        case 7:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement7.add_RightTapped), new Action<EventRegistrationToken>(uiElement7.remove_RightTapped), new RightTappedEventHandler(this.MessageThumb_RightTapped));
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement8.add_Holding), new Action<EventRegistrationToken>(uiElement8.remove_Holding), new HoldingEventHandler(this.MessageThumb_Holding));
          break;
        case 8:
          UIElement uiElement9 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement9.add_Tapped), new Action<EventRegistrationToken>(uiElement9.remove_Tapped), new TappedEventHandler(this.viewButton_Tapped));
          break;
        case 9:
          UIElement uiElement10 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement10.add_Tapped), new Action<EventRegistrationToken>(uiElement10.remove_Tapped), new TappedEventHandler(this.ContentControl_Tapped));
          break;
        case 10:
          UIElement uiElement11 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement11.add_Tapped), new Action<EventRegistrationToken>(uiElement11.remove_Tapped), new TappedEventHandler(this.pollAddButton_Tapped));
          break;
        case 11:
          UIElement uiElement12 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement12.add_Tapped), new Action<EventRegistrationToken>(uiElement12.remove_Tapped), new TappedEventHandler(this.actionAddButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
