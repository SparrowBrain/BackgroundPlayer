using System;

namespace BackgroundPlayer.Infrastructure
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}