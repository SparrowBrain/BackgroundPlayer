using System;

namespace BackgroundPlayer.Core.Infrastructure
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}