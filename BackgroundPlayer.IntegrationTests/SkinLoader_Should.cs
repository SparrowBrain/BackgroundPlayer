using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Xunit2;
using BackgroundPlayer.Configuration;
using Xunit;

namespace BackgroundPlayer.IntegrationTests
{
    public class SkinLoader_Should
    {
        public SkinLoader_Should()
        {
            
        }

        [Fact]
        public void LoadSkinWithImages()
        {
            var fixture = new Fixture();
            fixture.Register(() => ".\\skins");
            var skinLoader = fixture.Create<SkinLoader>();

            var skins = skinLoader.LoadSkins();

            Assert.NotEmpty(skins);
            var skin = skins.First();
            Assert.Equal(TimeSpan.FromMilliseconds(60000), skin.Duration);
            Assert.Null(skin.StartOffset.Month);
            Assert.Null(skin.StartOffset.Day);
            Assert.Null(skin.StartOffset.Hour);
            Assert.NotEmpty(skin.Images);
            Assert.Equal(2, skin.Images.Count);
        }
    }
}
