// Decompiled with JetBrains decompiler
// Type: myTube.SubscribedToPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class SubscribedToPage : Page, IComponentConnector
  {
    private bool select;
    private char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CollectionViewSource collection;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate smallChannelTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate largeChannelTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsPanelTemplate smallGroupPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsPanelTemplate largeGroupPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private SemanticZoom itemsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView zoomedInView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private GroupStyle groupStyle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView zoomedOutView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public SubscribedToPage()
    {
      this.InitializeComponent();
      YouTube.SubscriptionsLoaded += new EventHandler(this.YouTube_SubscriptionsLoaded);
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.SubscribedToPage_SizeChanged));
    }

    private void SubscribedToPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.groupStyle == null || this.zoomedInView == null)
        return;
      if (e.NewSize.Width > 700.0)
      {
        if (this.groupStyle.Panel != this.largeGroupPanel)
          this.groupStyle.put_Panel(this.largeGroupPanel);
        if (((ItemsControl) this.zoomedInView).ItemTemplate == this.largeChannelTemplate)
          return;
        ((ItemsControl) this.zoomedInView).put_ItemTemplate(this.largeChannelTemplate);
      }
      else
      {
        if (this.groupStyle.Panel != this.smallGroupPanel)
          this.groupStyle.put_Panel(this.smallGroupPanel);
        if (((ItemsControl) this.zoomedInView).ItemTemplate == this.smallChannelTemplate)
          return;
        ((ItemsControl) this.zoomedInView).put_ItemTemplate(this.smallChannelTemplate);
      }
    }

    private void SetItems()
    {
      this.select = false;
      try
      {
        List<SubscribedToPage.SubGroup> list = Enumerable.ToList<SubscribedToPage.SubGroup>(Enumerable.Select<IGrouping<string, UserInfo>, SubscribedToPage.SubGroup>(Enumerable.GroupBy<UserInfo, string>((IEnumerable<UserInfo>) YouTube.SubscriptionData, (Func<UserInfo, string>) (x =>
        {
          if (x.UserDisplayName.Length <= 0)
            return ".etc";
          char ch = x.UserDisplayName.ToUpper()[0];
          return Enumerable.Contains<char>((IEnumerable<char>) this.characters, ch) ? ch.ToString() : "#";
        })), (Func<IGrouping<string, UserInfo>, SubscribedToPage.SubGroup>) (x => new SubscribedToPage.SubGroup()
        {
          Title = x.Key.ToString(),
          Items = Enumerable.ToList<UserInfo>((IEnumerable<UserInfo>) x)
        })));
        list.Sort((Comparison<SubscribedToPage.SubGroup>) ((x, y) => x.Title.CompareTo(y.Title)));
        this.collection.put_Source((object) list);
        ((ItemsControl) this.zoomedOutView).put_ItemsSource((object) this.collection.View.CollectionGroups);
      }
      catch
      {
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e) => this.SetItems();

    private void YouTube_SubscriptionsLoaded(object sender, EventArgs e) => this.SetItems();

    private void Border_Tapped(object sender, TappedRoutedEventArgs e) => this.itemsControl.put_IsZoomedInViewActive(!this.itemsControl.IsZoomedInViewActive);

    private void zoomedInView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.select && ((Selector) this.zoomedInView).SelectedItem is UserInfo)
        ((App) Application.Current).RootFrame.Navigate(typeof (ChannelPage), (object) ((ClientDataBase) ((Selector) this.zoomedInView).SelectedItem).ID);
      ((Selector) this.zoomedInView).put_SelectedIndex(-1);
      this.select = true;
    }

    private void OverCanvas_ShownChanged(object sender, bool e)
    {
      ((UIElement) this.itemsControl).put_IsHitTestVisible(e);
      Ani.Begin((DependencyObject) this.itemsControl, "Opacity", e ? 1.0 : 0.0, 0.2);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SubscribedToPage.xaml"), (ComponentResourceLocation) 0);
      this.collection = (CollectionViewSource) ((FrameworkElement) this).FindName("collection");
      this.smallChannelTemplate = (DataTemplate) ((FrameworkElement) this).FindName("smallChannelTemplate");
      this.largeChannelTemplate = (DataTemplate) ((FrameworkElement) this).FindName("largeChannelTemplate");
      this.smallGroupPanel = (ItemsPanelTemplate) ((FrameworkElement) this).FindName("smallGroupPanel");
      this.largeGroupPanel = (ItemsPanelTemplate) ((FrameworkElement) this).FindName("largeGroupPanel");
      this.itemsControl = (SemanticZoom) ((FrameworkElement) this).FindName("itemsControl");
      this.zoomedInView = (ListView) ((FrameworkElement) this).FindName("zoomedInView");
      this.groupStyle = (GroupStyle) ((FrameworkElement) this).FindName("groupStyle");
      this.zoomedOutView = (ListView) ((FrameworkElement) this).FindName("zoomedOutView");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.OverCanvas_ShownChanged);
          break;
        case 2:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.zoomedInView_SelectionChanged));
          break;
        case 3:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.Border_Tapped));
          break;
      }
      this._contentLoaded = true;
    }

    private class SubGroup
    {
      public string Title { get; set; }

      public List<UserInfo> Items { get; set; }
    }
  }
}
