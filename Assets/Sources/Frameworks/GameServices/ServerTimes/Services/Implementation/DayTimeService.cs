using System;
using Sources.Frameworks.GameServices.ServerTimes.Services.Interfaces;

namespace Sources.Frameworks.GameServices.ServerTimes.Services.Implementation
{
    public class DayTimeService : ITimeService
    {
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
}