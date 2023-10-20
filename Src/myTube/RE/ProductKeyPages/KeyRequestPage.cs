// Decompiled with JetBrains decompiler
// Type: myTube.ProductKeyPages.KeyRequestPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.ProductKeyPages
{
  public sealed class KeyRequestPage : Page, IComponentConnector
  {
    private const int MaxCharacters = 512;
    private ProductKeyClient client;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel enterKeyPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel requestKeyPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock alreadyHaveKey;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock pageDesc;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock notAccepting;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock rejected;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock descTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock textCounter;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox descBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock awaitingApproval;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock keyApproved;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock key;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl submitButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock keyTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox keyBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl keyButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock requestAKey;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock requestAKeyDesc;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public KeyRequestPage()
    {
      this.InitializeComponent();
      this.textCounter.put_Text(this.descBox.Text.Length.ToString() + " / " + (object) 512);
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
      this.client = new ProductKeyClient();
    }

    private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (this.descBox.Text.Length < 512 || e.Key == 8 || this.descBox.SelectionLength != 0)
        return;
      e.put_Handled(true);
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      ((UIElement) this).put_Opacity(0.5);
      ((UIElement) this).put_IsHitTestVisible(false);
      if (e.NavigationMode != 1)
      {
        if (Settings.ProductKeyRequestId != null && Settings.RykenUserID != null)
        {
          try
          {
            ProductKey key = (ProductKey) null;
            try
            {
              key = await this.client.GetProductKey(Settings.ProductKeyRequestId, Settings.RykenUserID);
              if (key.Key != null)
                Settings.ProductKey = key.Key;
            }
            catch
            {
            }
            int num = await this.client.IsAcceptingProductKeyRequests() ? 1 : 0;
            this.SetPage(key, num != 0);
            key = (ProductKey) null;
          }
          catch
          {
          }
        }
        else
        {
          try
          {
            this.SetPage((ProductKey) null, await this.client.IsAcceptingProductKeyRequests());
          }
          catch
          {
          }
        }
      }
      Ani.Begin((DependencyObject) this, "Opacity", 1.0, 0.3);
      ((UIElement) this).put_IsHitTestVisible(true);
      base.OnNavigatedTo(e);
    }

    private void SetPage(ProductKey key, bool acceptingProductKeyRequests)
    {
      bool flag1 = true;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = true;
      if (key != null)
      {
        flag3 = key.Rejected;
        flag1 = key.Id == null || key.Rejected;
        this.descBox.put_Text(key.Description);
        flag2 = key.Id != null && key.Key == null && !key.Rejected;
        flag4 = key.Key != null;
        if (key.Key != null)
        {
          this.key.put_Text(key.Key);
          flag6 = false;
        }
        flag5 = true;
      }
      ContentControl submitButton = this.submitButton;
      Visibility visibility1;
      ((UIElement) this.pageDesc).put_Visibility((Visibility) (int) (visibility1 = flag1 & acceptingProductKeyRequests ? (Visibility) 0 : (Visibility) 1));
      Visibility visibility2 = visibility1;
      ((UIElement) submitButton).put_Visibility(visibility2);
      ((UIElement) this.awaitingApproval).put_Visibility(flag2 ? (Visibility) 0 : (Visibility) 1);
      ((UIElement) this.notAccepting).put_Visibility(acceptingProductKeyRequests ? (Visibility) 1 : (Visibility) 0);
      TextBlock keyApproved = this.keyApproved;
      Visibility visibility3;
      ((UIElement) this.key).put_Visibility((Visibility) (int) (visibility3 = flag4 ? (Visibility) 0 : (Visibility) 1));
      Visibility visibility4 = visibility3;
      ((UIElement) keyApproved).put_Visibility(visibility4);
      ((Control) this.descBox).put_IsEnabled(flag1);
      TextBox descBox = this.descBox;
      TextBlock descTitle = this.descTitle;
      Visibility visibility5;
      ((UIElement) this.textCounter).put_Visibility((Visibility) (int) (visibility5 = flag4 ? (Visibility) 1 : (Visibility) 0));
      Visibility visibility6;
      Visibility visibility7 = visibility6 = visibility5;
      ((UIElement) descTitle).put_Visibility(visibility6);
      Visibility visibility8 = visibility7;
      ((UIElement) descBox).put_Visibility(visibility8);
      ((UIElement) this.requestKeyPanel).put_Visibility(flag5 ? (Visibility) 0 : (Visibility) 1);
      ((UIElement) this.enterKeyPanel).put_Visibility(!flag5 ? (Visibility) 0 : (Visibility) 1);
      ((UIElement) this.rejected).put_Visibility(flag3 ? (Visibility) 0 : (Visibility) 1);
      TextBlock requestAkey = this.requestAKey;
      Visibility visibility9;
      ((UIElement) this.requestAKeyDesc).put_Visibility((Visibility) (int) (visibility9 = acceptingProductKeyRequests ? (Visibility) 0 : (Visibility) 1));
      Visibility visibility10 = visibility9;
      ((UIElement) requestAkey).put_Visibility(visibility10);
      ((UIElement) this.alreadyHaveKey).put_Visibility(flag6 ? (Visibility) 0 : (Visibility) 1);
    }

    private void descBox_TextChanged(object sender, TextChangedEventArgs e) => this.textCounter.put_Text(this.descBox.Text.Length.ToString() + " / " + (object) 512);

    private async void ContentControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((UIElement) this).put_IsHitTestVisible(false);
      if (this.descBox.Text.Length < 50 || string.IsNullOrWhiteSpace(this.descBox.Text))
      {
        IUICommand iuiCommand = await new MessageDialog("Your description is a bit too short. Please add some more information to it.", "Hold on").ShowAsync();
        ((UIElement) this).put_IsHitTestVisible(true);
        ((Control) this.descBox).Focus((FocusState) 3);
      }
      else
      {
        MessageDialog messageDialog = new MessageDialog("Are you sure you want to request a product key with this description?");
        UICommand yes = new UICommand("yes");
        UICommand uiCommand = new UICommand("no");
        messageDialog.Commands.Add((IUICommand) yes);
        messageDialog.Commands.Add((IUICommand) uiCommand);
        if (await messageDialog.ShowAsync() == yes)
        {
          ProductKey key = new ProductKey();
          key.Description = this.descBox.Text;
          key.Product = "myTube";
          try
          {
            if (Settings.RykenUserID == null)
              await App.CheckRykenUser();
            key.UserId = Settings.RykenUserID != null ? Settings.RykenUserID : throw new NullReferenceException("The Ryken user ID is null");
            ProductKey key1 = await this.client.RequestKey(key);
            if (key1 != null)
            {
              Settings.ProductKeyRequestId = key1.Id;
              this.SetPage(key1, true);
              IUICommand iuiCommand = await new MessageDialog("Your request has been sent successfully.", "Success").ShowAsync();
            }
            else
            {
              IUICommand iuiCommand1 = await new MessageDialog("We weren't able to process your request.", "Error").ShowAsync();
            }
          }
          catch (Exception ex)
          {
            new MessageDialog("We weren't able to process your request. Please check your connection and try again.", "Error").ShowAsync();
          }
          key = (ProductKey) null;
        }
        ((UIElement) this).put_IsHitTestVisible(true);
      }
    }

    private async void keyButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.keyBox.Text))
      {
        IUICommand iuiCommand = await new MessageDialog("Please enter a valid product key", "Enter a key").ShowAsync();
        ((Control) this.keyBox).Focus((FocusState) 3);
      }
      else
      {
        ((UIElement) this.keyButton).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) this.keyButton, "Opacity", 0.5, 0.2);
        try
        {
          ProductKey key = await this.client.ActivateProduct(this.keyBox.Text.Trim().Replace("-", "").ToLower(), "myTube");
          if (key == null)
          {
            IUICommand iuiCommand1 = await new MessageDialog("The submitted key is invalid", "Invalid key").ShowAsync();
          }
          else
          {
            Settings.ProductKeyRequestId = key.Id;
            Settings.ProductKey = key.Key;
            bool accepting = false;
            try
            {
              accepting = await this.client.IsAcceptingProductKeyRequests();
            }
            catch
            {
            }
            IUICommand iuiCommand2 = await new MessageDialog("The submitted key is valid and the full version of the app has been activated.", "Got it!").ShowAsync();
            this.SetPage(key, accepting);
          }
          key = (ProductKey) null;
        }
        catch
        {
        }
        Ani.Begin((DependencyObject) this.keyButton, "Opacity", 1.0, 0.2);
        ((UIElement) this.keyButton).put_IsHitTestVisible(true);
      }
    }

    private void requestAKey_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((UIElement) this.requestKeyPanel).put_Visibility((Visibility) 0);
      ((UIElement) this.enterKeyPanel).put_Visibility((Visibility) 1);
    }

    private void alreadyHaveKey_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((UIElement) this.requestKeyPanel).put_Visibility((Visibility) 1);
      ((UIElement) this.enterKeyPanel).put_Visibility((Visibility) 0);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ProductKeyPages/KeyRequestPage.xaml"), (ComponentResourceLocation) 0);
      this.enterKeyPanel = (StackPanel) ((FrameworkElement) this).FindName("enterKeyPanel");
      this.requestKeyPanel = (StackPanel) ((FrameworkElement) this).FindName("requestKeyPanel");
      this.alreadyHaveKey = (TextBlock) ((FrameworkElement) this).FindName("alreadyHaveKey");
      this.pageDesc = (TextBlock) ((FrameworkElement) this).FindName("pageDesc");
      this.notAccepting = (TextBlock) ((FrameworkElement) this).FindName("notAccepting");
      this.rejected = (TextBlock) ((FrameworkElement) this).FindName("rejected");
      this.descTitle = (TextBlock) ((FrameworkElement) this).FindName("descTitle");
      this.textCounter = (TextBlock) ((FrameworkElement) this).FindName("textCounter");
      this.descBox = (TextBox) ((FrameworkElement) this).FindName("descBox");
      this.awaitingApproval = (TextBlock) ((FrameworkElement) this).FindName("awaitingApproval");
      this.keyApproved = (TextBlock) ((FrameworkElement) this).FindName("keyApproved");
      this.key = (TextBlock) ((FrameworkElement) this).FindName("key");
      this.submitButton = (ContentControl) ((FrameworkElement) this).FindName("submitButton");
      this.keyTitle = (TextBlock) ((FrameworkElement) this).FindName("keyTitle");
      this.keyBox = (TextBox) ((FrameworkElement) this).FindName("keyBox");
      this.keyButton = (ContentControl) ((FrameworkElement) this).FindName("keyButton");
      this.requestAKey = (TextBlock) ((FrameworkElement) this).FindName("requestAKey");
      this.requestAKeyDesc = (TextBlock) ((FrameworkElement) this).FindName("requestAKeyDesc");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.alreadyHaveKey_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement2.add_KeyDown), new Action<EventRegistrationToken>(uiElement2.remove_KeyDown), new KeyEventHandler(this.TextBox_KeyDown));
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.descBox_TextChanged));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.ContentControl_Tapped));
          break;
        case 4:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.keyButton_Tapped));
          break;
        case 5:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.requestAKey_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
