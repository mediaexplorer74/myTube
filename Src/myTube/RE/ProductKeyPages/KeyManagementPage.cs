// Decompiled with JetBrains decompiler
// Type: myTube.ProductKeyPages.KeyManagementPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using Microsoft.CSharp.RuntimeBinder;
using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.ProductKeyPages
{
  public sealed class KeyManagementPage : Page, IComponentConnector
  {
    private ObservableCollection<ProductKey> keys = new ObservableCollection<ProductKey>();
    private ProductKeyClient client = new ProductKeyClient();
    private int page;
    private int activated;
    private int unactivated;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton AcceptButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton RejectButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView list;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar loading;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl activatedControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl unactivatedControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public KeyManagementPage()
    {
      this.InitializeComponent();
      ((ItemsControl) this.list).put_ItemsSource((object) this.keys);
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1)
        this.Refresh();
      base.OnNavigatedTo(e);
    }

    private async void Load()
    {
      if (this.client.IsBusy)
        return;
      this.loading.put_IsIndeterminate(true);
      try
      {
        ProductKey[] unactivatedKeys = await this.client.GetUnactivatedKeys(this.page, 20);
        ++this.page;
        foreach (ProductKey productKey1 in unactivatedKeys)
        {
          ProductKey productKey2 = (ProductKey) null;
          foreach (ProductKey key in (Collection<ProductKey>) this.keys)
          {
            if (key.Id == productKey1.Id)
            {
              productKey2 = key;
              break;
            }
          }
          if (productKey2 != null)
            ((Collection<ProductKey>) this.keys).Remove(productKey2);
          ((Collection<ProductKey>) this.keys).Add(productKey1);
        }
      }
      catch (Exception ex)
      {
        MessageDialog messageDialog = new MessageDialog(ex.ToString(), "Exception loading keys");
      }
      this.loading.put_IsIndeterminate(false);
    }

    private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
      if (this.scroll.ScrollableHeight - this.scroll.VerticalOffset >= 1000.0)
        return;
      this.Load();
    }

    private void Refresh()
    {
      ((Collection<ProductKey>) this.keys).Clear();
      this.page = 0;
      this.Load();
      this.RefreshCounts();
    }

    private async void RefreshCounts()
    {
      KeyManagementPage keyManagementPage;
      try
      {
        keyManagementPage = this;
        int activated = keyManagementPage.activated;
        int num = await this.client.CountActivatedOrUnactivated(true);
        keyManagementPage.activated = num;
        keyManagementPage = (KeyManagementPage) null;
      }
      catch
      {
      }
      try
      {
        keyManagementPage = this;
        int unactivated = keyManagementPage.unactivated;
        int num = await this.client.CountActivatedOrUnactivated(false);
        keyManagementPage.unactivated = num;
        keyManagementPage = (KeyManagementPage) null;
      }
      catch
      {
      }
      object obj1 = (object) new ExpandoObject();
      object obj2 = (object) new ExpandoObject();
      // ISSUE: reference to a compiler-generated field
      if (KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Number", typeof (KeyManagementPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__0, obj1, this.activated);
      // ISSUE: reference to a compiler-generated field
      if (KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Number", typeof (KeyManagementPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__1.Target((CallSite) KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__1, obj2, this.unactivated);
      // ISSUE: reference to a compiler-generated field
      if (KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Title", typeof (KeyManagementPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__2, obj1, "activated");
      // ISSUE: reference to a compiler-generated field
      if (KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Title", typeof (KeyManagementPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__3.Target((CallSite) KeyManagementPage.\u003C\u003Eo__10.\u003C\u003Ep__3, obj2, "unactivated");
      ((FrameworkElement) this.activatedControl).put_DataContext(obj1);
      ((FrameworkElement) this.unactivatedControl).put_DataContext(obj2);
    }

    private string[] GetSelectedIds()
    {
      IList<object> selectedItems = ((ListViewBase) this.list).SelectedItems;
      string[] selectedIds = new string[selectedItems.Count];
      for (int index = 0; index < selectedIds.Length; ++index)
        selectedIds[index] = (selectedItems[index] as ProductKey).Id;
      return selectedIds;
    }

    private async void RejectButton_Click(object sender, RoutedEventArgs e)
    {
      MessageDialog messageDialog = new MessageDialog("Are you sure you want to reject these keys?", "Reject these keys?");
      UICommand yes = new UICommand("yes");
      UICommand uiCommand = new UICommand("no");
      messageDialog.Commands.Add((IUICommand) yes);
      messageDialog.Commands.Add((IUICommand) uiCommand);
      if (await messageDialog.ShowAsync() != yes)
        return;
      ((Control) this.AcceptButton).put_IsEnabled(false);
      this.loading.put_IsIndeterminate(true);
      string[] selectedIds = this.GetSelectedIds();
      if (selectedIds.Length != 0)
      {
        try
        {
          ProductKey[] productKeyArray = await this.client.RejectKeys(selectedIds);
          List<ProductKey> productKeyList = new List<ProductKey>();
          foreach (object selectedItem in (IEnumerable<object>) ((ListViewBase) this.list).SelectedItems)
            productKeyList.Add(selectedItem as ProductKey);
          using (List<ProductKey>.Enumerator enumerator = productKeyList.GetEnumerator())
          {
            while (enumerator.MoveNext())
              ((Collection<ProductKey>) this.keys).Remove(enumerator.Current);
          }
          this.Refresh();
        }
        catch (Exception ex)
        {
          new MessageDialog(ex.ToString()).ShowAsync();
        }
      }
      ((Control) this.AcceptButton).put_IsEnabled(true);
      this.loading.put_IsIndeterminate(false);
    }

    private async void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
      MessageDialog messageDialog = new MessageDialog("Are you sure you want to accept these keys?", "Accept these keys?");
      UICommand yes = new UICommand("yes");
      UICommand uiCommand = new UICommand("no");
      messageDialog.Commands.Add((IUICommand) yes);
      messageDialog.Commands.Add((IUICommand) uiCommand);
      if (await messageDialog.ShowAsync() != yes)
        return;
      this.loading.put_IsIndeterminate(true);
      ((Control) this.AcceptButton).put_IsEnabled(false);
      string[] selectedIds = this.GetSelectedIds();
      if (selectedIds.Length != 0)
      {
        try
        {
          ProductKey[] productKeyArray = await this.client.ActivateKeys(selectedIds);
          List<ProductKey> productKeyList = new List<ProductKey>();
          foreach (object selectedItem in (IEnumerable<object>) ((ListViewBase) this.list).SelectedItems)
            productKeyList.Add(selectedItem as ProductKey);
          using (List<ProductKey>.Enumerator enumerator = productKeyList.GetEnumerator())
          {
            while (enumerator.MoveNext())
              ((Collection<ProductKey>) this.keys).Remove(enumerator.Current);
          }
          this.Refresh();
        }
        catch (Exception ex)
        {
          new MessageDialog(ex.ToString()).ShowAsync();
        }
      }
      ((Control) this.AcceptButton).put_IsEnabled(true);
      this.loading.put_IsIndeterminate(false);
      this.RefreshCounts();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ProductKeyPages/KeyManagementPage.xaml"), (ComponentResourceLocation) 0);
      this.AcceptButton = (AppBarButton) ((FrameworkElement) this).FindName("AcceptButton");
      this.RejectButton = (AppBarButton) ((FrameworkElement) this).FindName("RejectButton");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.list = (ListView) ((FrameworkElement) this).FindName("list");
      this.loading = (ProgressBar) ((FrameworkElement) this).FindName("loading");
      this.activatedControl = (ContentControl) ((FrameworkElement) this).FindName("activatedControl");
      this.unactivatedControl = (ContentControl) ((FrameworkElement) this).FindName("unactivatedControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.AcceptButton_Click));
          break;
        case 2:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.RejectButton_Click));
          break;
        case 3:
          ScrollViewer scrollViewer = (ScrollViewer) target;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.ScrollViewer_ViewChanged));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
