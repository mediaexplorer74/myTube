// myTube.MessageDialogs.PollDataClient


using myTube.Cloud;
using System;
using System.Threading.Tasks;

namespace myTube.MessageDialogs
{
    internal class PollDataClient
    {
        public PollDataClient()
        {
        }

        internal async Task<PollData> GetPoll(string pollId)
        {
            throw new NotImplementedException();
        }

        internal async Task<int> UserVotedOnPoll(string id, string rykenUserID)
        {
            throw new NotImplementedException();
        }

        internal async Task<int> Vote(string id, string rykenUserID, int selectedIndex)
        {
            throw new NotImplementedException();
        }
    }
}