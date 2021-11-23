using System;
using System.Threading.Tasks;
using BackgroundPlayer.Infrastructure;

namespace BackgroundPlayer
{
    public class Pacer : IPacer
    {
        public async Task Delay(TimeSpan time)
        {
            await Task.Delay(time);
        }
    }
}