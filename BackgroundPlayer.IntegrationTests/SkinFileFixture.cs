using AutoFixture;
using AutoFixture.Kernel;
using BackgroundPlayer.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackgroundPlayer.IntegrationTests
{
    public class SkinFileFixture : IDisposable
    {
        public SkinConfig SkinConfig { get; }
        public Fixture Fixture { get; }
        public string SkinsPath { get; }
        public IEnumerable<string> ImageFiles { get; }
        public string SkinName => "testSkin01";

        public SkinFileFixture()
        {
            Fixture = new Fixture();

            SkinConfig = Fixture.Create<SkinConfig>();
            SkinsPath = Fixture.Create<Settings>().SkinsPath;
            var skinJson = JsonConvert.SerializeObject(SkinConfig);
            var skinPath = Path.Combine(SkinsPath, SkinName);
            Directory.CreateDirectory(skinPath);
            File.WriteAllText(Path.Combine(skinPath, "skin.json"), skinJson);
            var imagePath = Path.Combine(skinPath, "images");
            Directory.CreateDirectory(imagePath);

            ImageFiles = Fixture.CreateMany<string>().Select(x => Path.Combine(imagePath, x + ".jpg")).OrderBy(x => x);
            foreach (var image in ImageFiles)
            {
                File.WriteAllText(image, string.Empty);
            }

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(ISkinValidator),
                    typeof(SkinValidator)));
        }

        public void Dispose()
        {
            Directory.Delete(SkinsPath, true);
        }
    }
}