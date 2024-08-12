using System;

namespace Sources.Frameworks.GameServices.ServerTimes.Services
{
    public interface IServerTimeService
    {
        public DateTime GetNetworkTime();
    }
}