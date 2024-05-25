// myTube.MessageDialogs.CommentThumb

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.MessageDialogs
{
  public sealed partial class CommentThumb : UserControl
  {
   
    public CommentThumb()
    {
      //this.InitializeComponent();
      ((Control) this).FontFamily = ((Control) DefaultPage.Current).FontFamily;
    }

  }
}
