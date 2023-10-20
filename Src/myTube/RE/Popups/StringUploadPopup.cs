// Decompiled with JetBrains decompiler
// Type: myTube.Popups.StringUploadPopup
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
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

namespace myTube.Popups
{
  public sealed class StringUploadPopup : UserControl, IComponentConnector
  {
    private string uploadString;
    private Flyout flyout;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox idBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox passwordBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button uploadButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button viewButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public string UploadString
    {
      get => this.uploadString;
      set => this.uploadString = value;
    }

    public StringUploadPopup()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.StringUploadPopup_Loaded));
    }

    private void StringUploadPopup_Loaded(object sender, RoutedEventArgs e) => ((Control) this.idBox).Focus((FocusState) 2);

    private async Task<bool> validate()
    {
      if (string.IsNullOrWhiteSpace(this.idBox.Text))
      {
        IUICommand iuiCommand = await new MessageDialog("Enter an ID given to you by the developer").ShowAsync();
        this.show();
        ((Control) this.idBox).Focus((FocusState) 2);
        return false;
      }
      if (this.passwordBox.Text.Trim().ToLower() == "giraffe")
        return true;
      IUICommand iuiCommand1 = await new MessageDialog("Invalid password").ShowAsync();
      this.show();
      ((Control) this.passwordBox).Focus((FocusState) 2);
      return false;
    }

    private async void uploadButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.uploadString == null)
        return;
      if (!await this.validate())
        return;
      StringClient stringClient = new StringClient();
      try
      {
        int num = await stringClient.AddString(new StringItem()
        {
          Text = this.uploadString,
          Language = this.idBox.Text.ToLower().Trim(),
          AppName = "myTube",
          Type = "Debug"
        }) ? 1 : 0;
        IUICommand iuiCommand = await new MessageDialog("Success").ShowAsync();
        this.close();
      }
      catch (Exception ex)
      {
        new MessageDialog("Error uploading data").ShowAsync();
      }
    }

    private void show()
    {
      if (this.flyout == null)
        return;
      ((FlyoutBase) this.flyout).ShowAt((FrameworkElement) DefaultPage.Current);
    }

    private void close()
    {
      if (this.flyout == null)
        return;
      ((FlyoutBase) this.flyout).Hide();
    }

    public static void Show(string uploadString)
    {
      StringUploadPopup stringUploadPopup = new StringUploadPopup()
      {
        UploadString = uploadString
      };
      Flyout flyout = new Flyout();
      flyout.put_Content((UIElement) stringUploadPopup);
      ((FlyoutBase) flyout).ShowAt((FrameworkElement) DefaultPage.Current);
      stringUploadPopup.flyout = flyout;
    }

    private async void viewButton_Click(object sender, RoutedEventArgs e)
    {
      if (!await this.validate())
        return;
      StringClient stringClient = new StringClient();
      try
      {
        string str = await stringClient.GetString("myTube", this.idBox.Text.ToLower().Trim(), "Debug");
        if (!string.IsNullOrWhiteSpace(str))
        {
          ViewString viewString = new ViewString()
          {
            Text = str
          };
          Flyout flyout = new Flyout();
          flyout.put_Content((UIElement) viewString);
          ((FlyoutBase) flyout).ShowAt((FrameworkElement) this);
        }
        else
        {
          IUICommand iuiCommand = await new MessageDialog("String is empty").ShowAsync();
          this.show();
        }
      }
      catch (Exception ex)
      {
        new MessageDialog("Error viewing data").ShowAsync();
      }
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/StringUploadPopup.xaml"), (ComponentResourceLocation) 0);
      this.idBox = (TextBox) ((FrameworkElement) this).FindName("idBox");
      this.passwordBox = (TextBox) ((FrameworkElement) this).FindName("passwordBox");
      this.uploadButton = (Button) ((FrameworkElement) this).FindName("uploadButton");
      this.viewButton = (Button) ((FrameworkElement) this).FindName("viewButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.uploadButton_Click));
          break;
        case 2:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.viewButton_Click));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
