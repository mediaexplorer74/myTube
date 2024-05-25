// myTube.MessageDialogs.MessageThumb

using myTube.Cloud;
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
  public sealed partial class MessageThumb : UserControl
  {
    public bool PerformTapMethod = true;


        public MessageThumb()
        {
            //this.InitializeComponent();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
          if (!this.PerformTapMethod || !(((FrameworkElement) this).DataContext is Message))
            return;
          App.ShowMessage((Message) ((FrameworkElement) this).DataContext);
        }

    
  }
}
