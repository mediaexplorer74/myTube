// Decompiled with JetBrains decompiler
// Type: myTube.Popups.SearchFilterUI
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed class SearchFilterUI : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox uploadDate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox sortBy;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public YouTubeTime UploadDate
    {
      get => (YouTubeTime) ((Selector) this.uploadDate).SelectedIndex;
      set => ((Selector) this.uploadDate).put_SelectedIndex((int) value);
    }

    public YouTubeOrder OrderBy
    {
      get => (YouTubeOrder) ((Selector) this.sortBy).SelectedIndex;
      set => ((Selector) this.sortBy).put_SelectedIndex((int) value);
    }

    public SearchFilterUI()
    {
      this.InitializeComponent();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/SearchFilterUI.xaml"), (ComponentResourceLocation) 0);
      this.uploadDate = (ComboBox) ((FrameworkElement) this).FindName("uploadDate");
      this.sortBy = (ComboBox) ((FrameworkElement) this).FindName("sortBy");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
