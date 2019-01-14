using System;
using System.Threading.Tasks;

namespace BackgroundPlayer
{
    public interface IPacer
    {
        Task Delay(TimeSpan time);
    }
}