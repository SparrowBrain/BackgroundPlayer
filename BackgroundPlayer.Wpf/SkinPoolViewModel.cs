using Stylet;
using System;

namespace BackgroundPlayer.Wpf
{
    public class SkinPoolViewModel : Screen
    {
        public SkinPoolViewModel()
        {

        }

        public BindableCollection<SkinDetailsViewModel> Skins { get; set; }

    }

    public class SkinDetailsViewModel : Screen
    {
        public TimeSpan Duration { get; set; }

        public int? OffsetMonth { get; set; }

        public int? OffsetDay { get; set; }

        public int? OffsetHour { get; set; }
    }
}