﻿using AutoFixture;
using AutoFixture.Xunit2;
using BackgroundPlayer.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            skin.StartOffset.Month = null;
            skin.StartOffset.Day = null;
            skin.StartOffset.Hour = null;

            var delay = skinCalculator.NextDelay(skin, start);

            Assert.Equal(TimeSpan.FromMilliseconds(expectedDelayMillisecods), delay);
        }

        [Theory]
        [AutoMoqData]
        public void ReturnNextImagesInOrder(SkinCalculator skinCalculator, Skin skin, DateTime start)
        {
            skin.StartOffset.Month = null;
            skin.StartOffset.Day = null;
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
        public void CalculateNextDelayToAccomodateForTheHourOffset(int imageCount, int durationHours, int offsetHour, DateTime start, DateTime now, int expectedDelayMinutes, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
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
            skin.StartOffset.Month = null;
            skin.StartOffset.Day = null;
            skin.StartOffset.Hour = offsetHour;

            var delay = skinCalculator.NextDelay(skin, start);

            Assert.Equal(TimeSpan.FromMinutes(expectedDelayMinutes), delay);
        }

        [Theory]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:00:00", 0)]
        [InlineAutoMoqData(12, 12, 0, "2019-01-12T00:00:00", "2019-01-12T00:30:00", 0)]
        [InlineAutoMoqData(12, 12, 6, "2019-01-12T13:22:06", "2019-01-12T16:30:00", 10)]
        public void ReturnNextImageWhileAccomodatingTheHourOffset(int imageCount, int durationHours, int offsetHour, DateTime start, DateTime now, int imageIndex, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
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
            skin.StartOffset.Month = null;
            skin.StartOffset.Day = null;
            skin.StartOffset.Hour = offsetHour;

            var nextImages = skinCalculator.NextImage(skin, start);

            Assert.Equal(skin.Images[imageIndex], nextImages.First());
        }

        [Theory]
        [InlineAutoMoqData(30, 30, 1, 1, "2019-01-01T00:00:00", "2019-01-01T00:00:00", 1 * 24)]
        [InlineAutoMoqData(3, 30, 1, 1, "2019-01-01T00:00:00", "2019-01-01T00:00:00", 10 * 24)]
        [InlineAutoMoqData(3, 30, 1, 1, "2019-01-01T00:00:00", "2019-01-05T12:00:00", 5.5 * 24)]
        [InlineAutoMoqData(3, 30, 1, 2, "2019-01-01T00:00:00", "2019-01-05T00:00:00", 7 * 24)]
        public void CalculateNextDelayToAccomodateForTheDayOffset(int imageCount, int durationDays, int offsetMonth, int offsetDay, DateTime start, DateTime now, int expectedDelayHours, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromDays(durationDays));
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
            skin.StartOffset.Month = offsetMonth;
            skin.StartOffset.Day = offsetDay;
            skin.StartOffset.Hour = null;

            var delay = skinCalculator.NextDelay(skin, start);

            Assert.Equal(TimeSpan.FromHours(expectedDelayHours), delay);
        }

        [Theory]
        [InlineAutoMoqData(30, 30, 1, 1, "2019-01-01T00:00:00", "2019-01-12T00:00:00", 11)]
        [InlineAutoMoqData(3, 30, 1, 1, "2019-01-01T00:00:00", "2019-01-12T00:00:00", 1)]
        [InlineAutoMoqData(12, 365, 1, 1, "2019-02-01T00:00:00", "2019-06-12T00:00:00", 5)]
        [InlineAutoMoqData(12, 365, 3, 1, "2019-05-01T00:00:00", "2019-06-12T00:00:00", 3)]
        public void ReturnNextImageWhileAccomodatingTheDayOffset(int imageCount, int durationDays, int offsetMonth, int offsetDay, DateTime start, DateTime now, int imageIndex, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromDays(durationDays));
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
            skin.StartOffset.Month = offsetMonth;
            skin.StartOffset.Day = offsetDay;
            skin.StartOffset.Hour = null;

            var nextImages = skinCalculator.NextImage(skin, start);

            Assert.Equal(skin.Images[imageIndex], nextImages.First());
        }

        [Theory]
        [InlineAutoMoqData(5, 5, 1, 1, "2019-01-01T00:00:00", "2019-01-01T00:00:00")]
        public void ReturnNextImagesInOrderWhileAccomodatingTheDayOffset(int imageCount, int durationDays, int offsetMonth, int offsetDay, DateTime start, DateTime now, int imageIndex, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromDays(durationDays));
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
            skin.StartOffset.Month = offsetMonth;
            skin.StartOffset.Day = offsetDay;
            skin.StartOffset.Hour = null;

            var nextImages = new List<string>();

            var day = 0;
            foreach (var nextImage in skinCalculator.NextImage(skin, start))
            {
                nextImages.Add(nextImage);
                day++;
                dateTimeProviderMock.Setup(x => x.Now()).Returns(now.AddDays(day));
            }

            Assert.Equal(skin.Images, nextImages);
        }

        [Theory]
        [InlineAutoMoqData(30, 30, 1, 1, "2019-01-01T00:00:00", "2019-06-12T00:00:00")]
        public void ReturnNoImageWhenDurationWasExceeded(int imageCount, int durationDays, int offsetMonth, int offsetDay, DateTime start, DateTime now, [Frozen] Mock<IDateTimeProvider> dateTimeProviderMock, SkinCalculator skinCalculator)
        {
            var fixture = new Fixture();
            dateTimeProviderMock.Setup(x => x.Now()).Returns(now);
            fixture.Register(() => TimeSpan.FromDays(durationDays));
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
            skin.StartOffset.Month = offsetMonth;
            skin.StartOffset.Day = offsetDay;
            skin.StartOffset.Hour = null;

            var nextImages = skinCalculator.NextImage(skin, start);

            Assert.Empty(nextImages);
        }
    }
}