using System;
using System.Threading.Tasks;

namespace BackgroundPlayer.Infrastructure
{
    public interface IPacer
    {
        Task Delay(TimeSpan time);
    }
}