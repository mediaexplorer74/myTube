// Decompiled with JetBrains decompiler
// Type: myTube.ExceptionPages.ExceptionDetails
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.ExceptionPages
{
  public sealed class ExceptionDetails : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton deleteButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock versions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock devices;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ExceptionDetails()
    {
      this.InitializeComponent();
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(ExceptionDetails_DataContextChanged)));
    }

    private void ExceptionDetails_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is ExceptionData dataContext))
        return;
      if (dataContext.Versions.Count > 0)
        this.versions.put_Text(string.Join("\n", (IEnumerable<string>) dataContext.Versions));
      else
        this.versions.put_Text("none");
      if (dataContext.Devices.Count > 0)
        this.devices.put_Text(string.Join("\n", (IEnumerable<string>) dataContext.Devices));
      else
        this.devices.put_Text("none");
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter != ((FrameworkElement) this).DataContext)
        ((FrameworkElement) this).put_DataContext(e.Parameter);
      base.OnNavigatedTo(e);
    }

    private async void deleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (Settings.UserMode >= UserMode.Owner)
      {
        ((Control) this.deleteButton).put_IsEnabled(false);
        if (((FrameworkElement) this).DataContext is ExceptionData)
        {
          try
          {
            if (await new MessageDialog("Are you sure you want to delete this exception?", "Are you sure?").ShowAsync("yes", "no") == 0)
            {
              if (await new ExceptionClient().Delete((((FrameworkElement) this).DataContext as ExceptionData).Id))
                App.Instance.RootFrame.GoBack();
            }
          }
          catch
          {
          }
        }
        ((Control) this.deleteButton).put_IsEnabled(true);
      }
      else
      {
        IUICommand iuiCommand = await new MessageDialog("You don't have permission to perform this action.").ShowAsync();
      }
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ExceptionPages/ExceptionDetails.xaml"), (ComponentResourceLocation) 0);
      this.deleteButton = (AppBarButton) ((FrameworkElement) this).FindName("deleteButton");
      this.versions = (TextBlock) ((FrameworkElement) this).FindName("versions");
      this.devices = (TextBlock) ((FrameworkElement) this).FindName("devices");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        ButtonBase buttonBase = (ButtonBase) target;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.deleteButton_Click));
      }
      this._contentLoaded = true;
    }
  }
}
