using BackgroundPlayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackgroundPlayer.Configuration
{
    public class SkinLoader
    {
        private const string SkinsPath = ".\\skins";
        private readonly ISkinValidator _skinValidator;

        public SkinLoader(ISkinValidator skinValidator)
        {
            _skinValidator = skinValidator;
        }

        public List<Skin> LoadSkins()
        {
            var skins = new List<Skin>();
            foreach (var skinFolder in Directory.EnumerateDirectories(SkinsPath))
            {
                var skinFile = Path.Combine(skinFolder, "skin.json");
                if (!File.Exists(skinFile))
                {
                    throw new Exception($"Skin {skinFile} does not exist!");
                }

                var name = Path.GetFileName(skinFolder);
                var skinJson = File.ReadAllText(skinFile);
                var skinConfig = JsonConvert.DeserializeObject<SkinConfig>(skinJson);
                var imagesPath = Path.Combine(skinFolder, "images");
                var images = Directory.EnumerateFiles(imagesPath).Where(x => _skinValidator.ValidImageExtension(x));

                skins.Add(new Skin(name, images.ToList(), TimeSpan.FromMilliseconds(skinConfig.DurationMillisecods), skinConfig.StartOffset));
            }

            return skins;
        }
    }
}