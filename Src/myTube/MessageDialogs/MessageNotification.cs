// myTube.MessageDialogs.MessageNotification

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.MessageDialogs
{
  public sealed partial class MessageNotification : UserControl
  {   

    public MessageNotification()
    {
      //this.InitializeComponent();
      ((Control) this).FontFamily = ((Control) DefaultPage.Current).FontFamily;
    }

    private void Border_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.Handled = true;
      //DefaultPage.Current.ClosePopup();
    }

  }
}
