// Decompiled with JetBrains decompiler
// Type: myTube.ChannelHeader
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class ChannelHeader : UserControl, IComponentConnector
  {
    private bool open;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Default;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Open;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private RowDefinition secondRow;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ChannelHeader()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.ChannelHeader_Tapped));
    }

    private void ChannelHeader_Tapped(object sender, TappedRoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, this.open ? "Default" : "Open", true);
      this.open = !this.open;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ChannelHeader.xaml"), (ComponentResourceLocation) 0);
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.Default = (VisualState) ((FrameworkElement) this).FindName("Default");
      this.Open = (VisualState) ((FrameworkElement) this).FindName("Open");
      this.secondRow = (RowDefinition) ((FrameworkElement) this).FindName("secondRow");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
