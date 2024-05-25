// myTube.UserHome

using RykenTube;
using System.Threading.Tasks;

namespace myTube
{
    internal class ClientAndFirstLoadTask
    {
        public ClientAndFirstLoadTask()
        {
        }

        public VideoListClient Client { get; set; }
        public Task<YouTubeEntry[]> LoadTask { get; set; }
    }
}