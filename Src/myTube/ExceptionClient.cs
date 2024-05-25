using myTube.Cloud;
using System;
using System.Threading.Tasks;

namespace myTube
{
    internal class ExceptionClient
    {
        public ExceptionClient()
        {
        }

        internal async Task<bool> AddException(ExceptionData exception, string fullDeviceName, Version version)
        {
            throw new NotImplementedException();
        }
    }
}