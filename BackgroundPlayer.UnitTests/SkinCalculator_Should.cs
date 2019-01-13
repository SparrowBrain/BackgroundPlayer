using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace BackgroundPlayer.UnitTests
{
    public class SkinCalculator_Should
    {
        [Theory]
        [InlineAutoMoqData(1, 10, 10)]
        [InlineAutoMoqData(2, 10, 5)]
        public void CalculateDelayToSpreadImagesAcrossTheDuration(int imageCount, int durationMilliseconds, int expectedDelayMillisecods, SkinCalculator skinCalculator, DateTime start)
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
            skin.StartOffset.Hour = null;

            var delay = skinCalculator.NextDelay(skin, start);

            Assert.Equal(TimeSpan.FromMilliseconds(expectedDelayMillisecods), delay);
        }

        [Theory]
        [AutoMoqData]
        public void ReturnNextImagesInOrder(SkinCalculator skinCalculator, Skin skin, DateTime start)
        {
            skin.StartOffset.Hour = null;

            var nextImages = skinCalculator.NextImage(skin, start);

            for (var i = 0; i < skin.Images.Count; i++)
            {
                Assert.Equal(skin.Images[i], nextImages.ElementAt(i));
            }
        }

        [Theory]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:00:00", 60)]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:30:00", 30)]
        [InlineAutoMoqData(12, 12, 6, "2019-01-12T13:22:06", "2019-01-12T16:30:00", 30)]
        public void CalculateNextDelayToAccomodateForTheOffsetInTheDay(int imageCount, int durationHours, int offsetHour, DateTime start, DateTime now, int expectedDelayMinutes, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromHours(durationHours));
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
            skin.StartOffset.Hour = offsetHour;

            var delay = skinCalculator.NextDelay(skin, start);

            Assert.Equal(TimeSpan.FromMinutes(expectedDelayMinutes), delay);
        }


        [Theory]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:00:00", 0)]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:30:00", 0)]
        [InlineAutoMoqData(12, 12, 6, "2019-01-12T13:22:06", "2019-01-12T16:30:00", 10)]
        public void ReturnNextImageWhileAccomodatingTheOffsetInTheDay(int imageCount, int durationHours, int offsetHour, DateTime start, DateTime now, int imageIndex, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromHours(durationHours));
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
            skin.StartOffset.Hour = offsetHour;

            var nextImages = skinCalculator.NextImage(skin, start);

            Assert.Equal(skin.Images[imageIndex], nextImages.First());
        }

    }
}