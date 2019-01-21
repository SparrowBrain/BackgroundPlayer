using AutoFixture;
using BackgroundPlayer.Configuration;
using System;
using System.Linq;
using Xunit;

namespace BackgroundPlayer.IntegrationTests
{
    public class SkinLoader_Should : IClassFixture<SkinFileFixture>
    {
        private readonly SkinFileFixture _skinFileFixture;

        public SkinLoader_Should(SkinFileFixture skinFileFixture)
        {
            _skinFileFixture = skinFileFixture;
        }

        [Fact]
        public void LoadSkinWithImages()
        {
            _skinFileFixture.Fixture.Register(() => _skinFileFixture.SkinsPath);
            var skinLoader = _skinFileFixture.Fixture.Create<SkinLoader>();

            var skins = skinLoader.LoadSkins();

            Assert.NotEmpty(skins);
            var skin = skins.First();
            Assert.Equal(_skinFileFixture.SkinName, skin.Name);
            Assert.Equal(TimeSpan.FromMilliseconds(_skinFileFixture.SkinConfig.DurationMillisecods), skin.Duration);
            Assert.Equal(_skinFileFixture.SkinConfig.StartOffset, skin.StartOffset);
            Assert.NotEmpty(skin.Images);
            Assert.Equal(_skinFileFixture.ImageFiles, skin.Images);
        }
    }
}