using System.Collections.Generic;
using BackgroundPlayer.Configuration;
using BackgroundPlayer.Model;

namespace BackgroundPlayer
{
    public class StartUp
    {
        private readonly SkinLoader _skinLoader;

        public StartUp(SkinLoader skinLoader)
        {
            _skinLoader = skinLoader;
        }

        public List<Skin> LoadSkins()
        {
            return _skinLoader.LoadSkins();
        }
    }
}