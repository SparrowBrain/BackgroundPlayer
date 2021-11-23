using System;
using System.Collections.Generic;

namespace BackgroundPlayer.Model
{
    public class Skin
    {
        public Skin(string name, IList<string> images, TimeSpan duration, StartOffset startOffset)
        {
            Name = name;
            Images = images;
            Duration = duration;
            StartOffset = startOffset;
            
        }

        public string Name { get; }

        public IList<string> Images { get; }

        public TimeSpan Duration { get; }

        public StartOffset StartOffset { get; }
        
    }
}