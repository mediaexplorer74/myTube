// Decompiled with JetBrains decompiler
// Type: myTube.Popups.ViewString
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed class ViewString : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock text;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public string Text
    {
      get => this.text.Text;
      set => this.text.put_Text(value);
    }

    public ViewString() => this.InitializeComponent();

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/ViewString.xaml"), (ComponentResourceLocation) 0);
      this.text = (TextBlock) ((FrameworkElement) this).FindName("text");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
