using System;
using System.Threading.Tasks;

namespace BackgroundPlayer.Core.Infrastructure
{
    public class Pacer : IPacer
    {
        public async Task Delay(TimeSpan time)
        {
            await Task.Delay(time);
        }
    }
}