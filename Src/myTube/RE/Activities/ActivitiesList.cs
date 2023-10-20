// Decompiled with JetBrains decompiler
// Type: myTube.Activities.ActivitiesList
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.Activities
{
  public sealed class ActivitiesList : UserControl, IComponentConnector
  {
    private YouTubeEntryCollection entries;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate defaultTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate upload;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate like;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate recommended;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl items;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ActivitiesList()
    {
      this.InitializeComponent();
      this.entries = new YouTubeEntryCollection();
      this.items.put_ItemsSource((object) this.entries);
      this.items.put_ItemTemplateSelector((DataTemplateSelector) new ActivityListTemplateSelector(this));
    }

    public void AddItems(IEnumerable<YouTubeEntry> items)
    {
      foreach (YouTubeEntry youTubeEntry in items)
        ((Collection<YouTubeEntry>) this.entries).Add(youTubeEntry);
    }

    public DataTemplate GetDataTemplate(YouTubeActivity type)
    {
      switch (type)
      {
        case YouTubeActivity.Upload:
          return this.upload;
        case YouTubeActivity.Recommended:
          return this.recommended;
        case YouTubeActivity.Like:
          return this.like;
        default:
          return this.defaultTemplate;
      }
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Activities/ActivitiesList.xaml"), (ComponentResourceLocation) 0);
      this.defaultTemplate = (DataTemplate) ((FrameworkElement) this).FindName("defaultTemplate");
      this.upload = (DataTemplate) ((FrameworkElement) this).FindName("upload");
      this.like = (DataTemplate) ((FrameworkElement) this).FindName("like");
      this.recommended = (DataTemplate) ((FrameworkElement) this).FindName("recommended");
      this.items = (ItemsControl) ((FrameworkElement) this).FindName("items");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
