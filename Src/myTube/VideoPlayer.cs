using RykenTube;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace myTube
{
    public class VideoPlayer
    {
        public YouTubeQuality CurrentQuality;
        public MediaElement MediaElement;

        internal async Task ChangeQuality(YouTubeQuality audio, bool v)
        {
            throw new NotImplementedException();
        }

        internal void DeregisterBackgroundEvent()
        {
            throw new NotImplementedException();
        }
    }
}