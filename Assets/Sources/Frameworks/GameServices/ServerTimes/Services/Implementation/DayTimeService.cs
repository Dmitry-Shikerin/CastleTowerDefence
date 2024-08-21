using System;

namespace Sources.Frameworks.GameServices.ServerTimes.Services
{
    public class DayTimeService : ITimeService
    {
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
}