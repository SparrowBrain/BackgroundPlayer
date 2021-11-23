using System.Collections.Generic;
using BackgroundPlayer.Core.Configuration;
using BackgroundPlayer.Core.Model;

namespace BackgroundPlayer.Core
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