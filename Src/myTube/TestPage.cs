// myTube.TestPage

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed partial class TestPage : Page
  {
    
    private VideoList videoList;
    
    
    public TestPage()
    {
      //this.InitializeComponent();
      this.NavigationCacheMode = ((NavigationCacheMode) 2);
      this.videoList.Client = (VideoListClient) new FeedClient(YouTubeFeed.Popular, Category.All, 
          YouTubeTime.Today, 40);
    }

    
  }
}
