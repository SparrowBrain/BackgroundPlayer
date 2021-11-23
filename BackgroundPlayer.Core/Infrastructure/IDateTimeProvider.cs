using System;

namespace BackgroundPlayer.Core.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }
}