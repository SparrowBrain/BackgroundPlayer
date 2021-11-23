using System.Collections.Generic;
using BackgroundPlayer.Core.Model;

namespace BackgroundPlayer.Core.Configuration
{
    public class Playlist
    {
        public bool Shuffle { get; set; }

        IEnumerable<Skin> Skins { get; set; }
    }
}
