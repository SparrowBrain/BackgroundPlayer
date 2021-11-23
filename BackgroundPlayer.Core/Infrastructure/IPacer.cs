using System;
using System.Threading.Tasks;

namespace BackgroundPlayer.Core.Infrastructure
{
    public interface IPacer
    {
        Task Delay(TimeSpan time);
    }
}