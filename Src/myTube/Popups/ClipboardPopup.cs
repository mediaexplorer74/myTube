// myTube.Popups.ClipboardPopup

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed partial class ClipboardPopup : UserControl
  {
    //TEMP
    private ContentControl closeButton = new ContentControl();
    

    public ClipboardPopup()
    {
       //this.InitializeComponent();
       this.Tapped += ClipboardPopup_Tapped;
    }

    private void ClipboardPopup_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.Handled = true;
      //DefaultPage.Current.ClosePopup();
      App.Instance.RootFrame.Navigate(typeof (VideoPage), ((FrameworkElement) this).DataContext);
    }

    private void closeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.Handled = true;
      //DefaultPage.Current.ClosePopup();
    }

    
  }
}
