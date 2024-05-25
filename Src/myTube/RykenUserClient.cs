using myTube.Cloud;
using System;
using System.Threading.Tasks;

namespace myTube
{
    public class RykenUserClient
    {
        public RykenUserClient()
        {
        }

        internal async Task<RykenUser> ChangeName(string id, string userName)
        {
            throw new NotImplementedException();
        }

        internal async Task<RykenUser> CreateNewUser(string userName)
        {
            throw new NotImplementedException();
        }

        internal async Task<RykenUser> CreateNewUser()
        {
            throw new NotImplementedException();
        }

        internal async Task<RykenUser> Login(string rykenUserID)
        {
            throw new NotImplementedException();
        }
    }
}