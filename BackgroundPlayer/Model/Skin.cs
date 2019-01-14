using System;
using System.Collections.Generic;

namespace BackgroundPlayer.Model
{
    public class Skin
    {
        public Skin(IList<string> images, TimeSpan duration, StartOffset startOffset)
        {
            Images = images;
            Duration = duration;
            StartOffset = startOffset;
        }

        public IList<string> Images { get; }
        public TimeSpan Duration { get; }
        public StartOffset StartOffset { get; }
    }
}