using System;

namespace BackgroundPlayer
{
    public class DateTimeProvider :IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}