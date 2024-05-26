// myTube.VideoPlayer

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
using RykenTube;
using System.Threading.Tasks;

namespace myTube
{
    public sealed partial class VideoPlayer
    {
        public YouTubeQuality CurrentQuality;
        public MediaElement MediaElement;

        internal bool ControlsShown;
        internal bool Hidden;
        internal bool MediaRunning;

        public async Task ChangeQuality(YouTubeQuality audio, bool v)
        {
            throw new NotImplementedException();
        }

        public void DeregisterBackgroundEvent()
        {
            throw new NotImplementedException();
        }
    }
}