using System;

namespace BackgroundPlayer
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}