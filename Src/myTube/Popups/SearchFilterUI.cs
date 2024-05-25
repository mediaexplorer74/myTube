// myTube.Popups.SearchFilterUI

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
  public sealed partial class SearchFilterUI : UserControl
  {
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox uploadDate;
    //[GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox sortBy;
    
    public YouTubeTime UploadDate
    {
      get => (YouTubeTime) ((Selector) this.uploadDate).SelectedIndex;
      set => ((Selector) this.uploadDate).SelectedIndex = (int) value;
    }

    public YouTubeOrder OrderBy
    {
      get => (YouTubeOrder) ((Selector) this.sortBy).SelectedIndex;
      set => ((Selector)this.sortBy).SelectedIndex = (int)value;
    }

    public SearchFilterUI()
    {
      //this.InitializeComponent();
      ((Control) this).FontFamily = ((Control) DefaultPage.Current).FontFamily;
    }

   
  }
}
