using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BackgroundPlayer.Model;
using Newtonsoft.Json;

namespace BackgroundPlayer.Configuration
{
    public class SkinLoader
    {
        private readonly Configuration _configuration;

        public SkinLoader(Configuration configuration)
        {
            _configuration = configuration;
        }

        public List<Skin> LoadSkins()
        {
            var skins = new List<Skin>();
            foreach (var skinFolder in Directory.EnumerateDirectories(_configuration.SkinsPath))
            {
                var skinFile = Path.Combine(skinFolder, "skin.json");
                if (!File.Exists(skinFile))
                {
                    throw new Exception($"Skin {skinFile} does not exist!");
                }

                var skinJson = File.ReadAllText(skinFile);
                var skinConfig = JsonConvert.DeserializeObject<SkinConfig>(skinJson);
                var imagesPath = Path.Combine(skinFolder, "images");
                var images = Directory.EnumerateFiles(imagesPath).Where(x=> new[] { ".png", ".jpg" }.Contains(Path.GetExtension(x)));

                skins.Add(new Skin(images.ToList(), TimeSpan.FromMilliseconds(skinConfig.DurationMillisecods), skinConfig.StartOffset));
            }

            return skins;
        }
    }
}