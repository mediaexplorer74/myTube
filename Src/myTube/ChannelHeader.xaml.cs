// myTube.ChannelHeader

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml.Markup;

namespace myTube
{
    public sealed partial class ChannelHeader : UserControl
    {
        private bool open;
       

        public ChannelHeader()
        {
            this.InitializeComponent();
            ((UIElement)this).Tapped += this.ChannelHeader_Tapped;
        }

        private void ChannelHeader_Tapped(object sender, TappedRoutedEventArgs e)
        {
            VisualStateManager.GoToState((Control)this, this.open ? "Default" : "Open", true);
            this.open = !this.open;
        }

    }
}

