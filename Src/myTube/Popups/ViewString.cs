// myTube.Popups.ViewString

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed partial class ViewString : UserControl
  {
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock text;


    public string Text
    {
      get => this.text.Text;
      set => this.text.Text = value;
    }

        public ViewString()
        {
            //this.InitializeComponent();
        }
    }
}
