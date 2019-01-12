using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using Xunit;

namespace BackgroundPlayer.UnitTests
{
    public class SkinCalculator_Should
    {
        [Theory]
        [InlineAutoData(1, 10, 10)]
        [InlineAutoData(2, 10, 5)]
        public void CalculateDelayToSpreadImagesAcrossTheDuration(int imageCount, int durationMilliseconds, int expectedDelayMillisecods, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            fixture.Register(() => TimeSpan.FromMilliseconds(durationMilliseconds));
            fixture.Register((Func<IList<string>>)(() =>
            {
                var images = new List<string>();
                for (var i = 0; i < imageCount; i++)
                {
                    images.Add(fixture.Create<string>());
                }
                return images;
            }));
            var skin = fixture.Create<Skin>();

            var delay = skinCalculator.NextDelay(skin);

            Assert.Equal(TimeSpan.FromMilliseconds(expectedDelayMillisecods), delay);
        }
    }
}