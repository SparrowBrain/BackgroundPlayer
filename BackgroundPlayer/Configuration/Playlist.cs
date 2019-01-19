using BackgroundPlayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundPlayer.Configuration
{
    public class Playlist
    {
        public bool Shuffle { get; set; }

        IEnumerable<Skin> Skins { get; set; }
    }
}
