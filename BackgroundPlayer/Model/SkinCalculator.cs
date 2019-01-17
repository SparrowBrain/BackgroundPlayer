using BackgroundPlayer.Infrastructure;
using System;
using System.Collections.Generic;

namespace BackgroundPlayer.Model
{
    public class SkinCalculator : ISkinCalculator
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SkinCalculator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public TimeSpan NextDelay(Skin skin, DateTime playbackStart)
        {
            var imageDuration = skin.Duration / skin.Images.Count;
            if (skin.StartOffset.Hour.HasValue || skin.StartOffset.Month.HasValue || skin.StartOffset.Day.HasValue)
            {
                var startAdjustedWithOffset = AdjustStartTimeWithOffset(skin, playbackStart);
                return TimeLeftInCurrentIteration(imageDuration, startAdjustedWithOffset);
            }

            return imageDuration;
        }

        private DateTime AdjustStartTimeWithOffset(Skin skin, DateTime playbackStart)
        {
            var startAdjustedWithOffset = new DateTime(playbackStart.Year,
                skin.StartOffset.Month ?? playbackStart.Month,
                skin.StartOffset.Day ?? playbackStart.Day,
                skin.StartOffset.Hour ?? playbackStart.Hour,
                00,
                00);

            var now = _dateTimeProvider.Now();
            if (startAdjustedWithOffset > now)
            {
                startAdjustedWithOffset = PushBackStartAdjustedWithOffset(startAdjustedWithOffset, now);
            }

            return startAdjustedWithOffset;
        }

        public IEnumerable<string> NextImage(Skin skin, DateTime playbackStart)
        {
            foreach (var image in skin.Images)
            {
                if (skin.StartOffset.Hour.HasValue || skin.StartOffset.Month.HasValue || skin.StartOffset.Day.HasValue)
                {
                    var startAdjustedWithOffset = AdjustStartTimeWithOffset(skin, playbackStart);
                    var index = GetImageIteration(skin, startAdjustedWithOffset);
                    if (index >= skin.Images.Count)
                    {
                        break;
                    }

                    yield return skin.Images[index];
                }
                else
                {
                    yield return image;
                }
            }
        }

        private TimeSpan TimeLeftInCurrentIteration(TimeSpan imageDuration, DateTime startAdjustedWithOffset)
        {
            var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
            return imageDuration -
                   TimeSpan.FromMilliseconds(timeSinceStart.TotalMilliseconds % imageDuration.TotalMilliseconds);
        }

        private int GetImageIteration(Skin skin, DateTime startAdjustedWithOffset)
        {
            var imageDuration = skin.Duration / skin.Images.Count;
            var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
            var index = (int)(timeSinceStart.TotalMilliseconds / imageDuration.TotalMilliseconds);
            return index;
        }

        private static DateTime PushBackStartAdjustedWithOffset(DateTime startAdjustedWithOffset, DateTime now)
        {
            var year = startAdjustedWithOffset.Year;
            var month = startAdjustedWithOffset.Month;
            var day = startAdjustedWithOffset.Day;

            if (startAdjustedWithOffset.Month > now.Month)
            {
                year--;
            }
            else if (startAdjustedWithOffset.Day > now.Day)
            {
                month--;
            }
            else if (startAdjustedWithOffset.Hour > now.Hour)
            {
                day--;
            }

            startAdjustedWithOffset = new DateTime(year,
                month,
                day,
                startAdjustedWithOffset.Hour,
                startAdjustedWithOffset.Minute,
                startAdjustedWithOffset.Second);

            return startAdjustedWithOffset;
        }
    }
}