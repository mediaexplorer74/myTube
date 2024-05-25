// myTube.SettingsContainer


using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed partial class SettingsContainer : UserControl
  {
    
    public SettingsContainer()
    {
      //this.InitializeComponent();
      ((Control) this).FontFamily = ((Control) DefaultPage.Current).FontFamily;
    }
   
  }
}
