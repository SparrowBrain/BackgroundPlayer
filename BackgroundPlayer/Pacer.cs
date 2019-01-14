using System;
using System.Threading.Tasks;

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