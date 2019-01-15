using BackgroundPlayer.Model;
using System;
using System.Collections.Generic;

namespace BackgroundPlayer
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

                var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
                return imageDuration - TimeSpan.FromMilliseconds(timeSinceStart.TotalMilliseconds % imageDuration.TotalMilliseconds);
            }

            return imageDuration;
        }

        public IEnumerable<string> NextImage(Skin skin, DateTime playbackStart)
        {
            foreach (var image in skin.Images)
            {
                if (skin.StartOffset.Hour.HasValue || skin.StartOffset.Month.HasValue || skin.StartOffset.Day.HasValue)
                {
                    var imageDuration = skin.Duration / skin.Images.Count;
                    var startAdjustedWithOffset = AdjustStartTimeWithOffset(skin, playbackStart);

                    var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
                    var index = (int)(timeSinceStart.TotalMilliseconds / imageDuration.TotalMilliseconds);
                    yield return skin.Images[index];
                }
                else
                {
                    yield return image;
                }
            }
        }

        private static DateTime AdjustStartTimeWithOffset(Skin skin, DateTime playbackStart)
        {
            var startAdjustedWithOffset = new DateTime(playbackStart.Year,
                skin.StartOffset.Month ?? playbackStart.Month,
                skin.StartOffset.Day ?? playbackStart.Day,
                skin.StartOffset.Hour ?? playbackStart.Hour,
                00,
                00);
            return startAdjustedWithOffset;
        }
    }
}