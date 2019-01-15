using System;
using System.Collections.Generic;

namespace BackgroundPlayer.Model
{
    public interface ISkinCalculator
    {
        TimeSpan NextDelay(Skin skin, DateTime playbackStart);

        IEnumerable<string> NextImage(Skin skin, DateTime playbackStart);
    }
}