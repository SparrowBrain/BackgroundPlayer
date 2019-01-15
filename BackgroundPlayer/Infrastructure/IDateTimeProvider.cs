using System;

namespace BackgroundPlayer.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}