// Decompiled with JetBrains decompiler
// Type: myTube.UserPages.UserManagement
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.UserPages
{
  public sealed class UserManagement : Page, IComponentConnector
  {
    private RykenUserClient client;
    private bool firstRefresh;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton refreshButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock inactive;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox inactivePicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button cleanUsers;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock allTime;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock year;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock month;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock week;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock day;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock hour;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock minute;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public UserManagement()
    {
      this.InitializeComponent();
      this.client = new RykenUserClient();
    }

    private async Task Refresh()
    {
      this.firstRefresh = true;
      ((Control) this.refreshButton).put_IsEnabled(false);
      try
      {
        int[] numArray = await this.client.ActiveUsersBatch(3650.0, 365.0, 31.0, 7.0, 1.0, 1.0 / 24.0, 0.00069444444444444436);
        this.allTime.put_Text(numArray[0].ToString());
        this.year.put_Text(numArray[1].ToString());
        this.month.put_Text(numArray[2].ToString());
        this.week.put_Text(numArray[3].ToString());
        this.day.put_Text(numArray[4].ToString());
        this.hour.put_Text(numArray[5].ToString());
        this.minute.put_Text(numArray[6].ToString());
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.inactive.put_Text((await this.client.InactiveUsersBatch((double) ((Selector) this.inactivePicker).SelectedItem))[0].ToString());
      }
      catch
      {
      }
      ((Control) this.refreshButton).put_IsEnabled(true);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1)
        this.Refresh();
      base.OnNavigatedTo(e);
    }

    private void refreshButton_Click(object sender, RoutedEventArgs e) => this.Refresh();

    private void overCanvas_ShownChanged(object sender, bool e)
    {
      if (e)
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 1);
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.Refresh();

    private async void cleanUsers_Click(object sender, RoutedEventArgs e)
    {
      ToAgoString toAgoString = new ToAgoString();
      double days = (double) ((Selector) this.inactivePicker).SelectedItem;
      // ISSUE: variable of a boxed type
      __Boxed<double> local = (ValueType) days;
      Type targetType = typeof (string);
      MessageDialog messageDialog1 = new MessageDialog("Are you sure you want to delete users from " + toAgoString.Convert((object) local, targetType, (object) "days", "en").ToString() + "?", "Delete these users?");
      UICommand yes = new UICommand("yes");
      UICommand uiCommand = new UICommand("no");
      messageDialog1.Commands.Add((IUICommand) yes);
      messageDialog1.Commands.Add((IUICommand) uiCommand);
      ((Control) this.cleanUsers).put_IsEnabled(false);
      if (await messageDialog1.ShowAsync() == yes)
      {
        try
        {
          IUICommand iuiCommand = await new MessageDialog((await this.client.DeleteOldUsers(days)).ToString() + " inactive users were cleaned from the service.", "Users cleaned").ShowAsync();
        }
        catch (Exception ex)
        {
          MessageDialog messageDialog2 = new MessageDialog("Error deleting users:\n" + ex.ToString());
        }
      }
      this.Refresh();
      ((Control) this.cleanUsers).put_IsEnabled(true);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///UserPages/UserManagement.xaml"), (ComponentResourceLocation) 0);
      this.refreshButton = (AppBarButton) ((FrameworkElement) this).FindName("refreshButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.inactive = (TextBlock) ((FrameworkElement) this).FindName("inactive");
      this.inactivePicker = (ComboBox) ((FrameworkElement) this).FindName("inactivePicker");
      this.cleanUsers = (Button) ((FrameworkElement) this).FindName("cleanUsers");
      this.allTime = (TextBlock) ((FrameworkElement) this).FindName("allTime");
      this.year = (TextBlock) ((FrameworkElement) this).FindName("year");
      this.month = (TextBlock) ((FrameworkElement) this).FindName("month");
      this.week = (TextBlock) ((FrameworkElement) this).FindName("week");
      this.day = (TextBlock) ((FrameworkElement) this).FindName("day");
      this.hour = (TextBlock) ((FrameworkElement) this).FindName("hour");
      this.minute = (TextBlock) ((FrameworkElement) this).FindName("minute");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.refreshButton_Click));
          break;
        case 2:
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          break;
        case 3:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.ComboBox_SelectionChanged));
          break;
        case 4:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.cleanUsers_Click));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
