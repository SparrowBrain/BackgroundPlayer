using Stylet;
using System;

namespace BackgroundPlayer.Wpf
{
    public class SkinDetailsViewModel : Screen
    {
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }

        public int? OffsetMonth { get; set; }

        public int? OffsetDay { get; set; }

        public int? OffsetHour { get; set; }
    }
}