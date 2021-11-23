using System;
using System.Collections.Generic;
using BackgroundPlayer.Core.Model;

namespace BackgroundPlayer.Core.Playback
{
    public interface ISkinCalculator
    {
        TimeSpan NextDelay(Skin skin, DateTime playbackStart);

        IEnumerable<string> NextImage(Skin skin, DateTime playbackStart);
    }
}