using BackgroundPlayer.Core.Infrastructure;
using BackgroundPlayer.Core.Model;
using System;
using System.Collections.Generic;

namespace BackgroundPlayer.Core.Playback
{
	public class SkinCalculator : ISkinCalculator
	{
		private const int MaxDelayInMinutes = 30;
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

				var now = _dateTimeProvider.Now();
				if (CheckSkinHasFinishedPlaying(skin, now, startAdjustedWithOffset))
				{
					var timeTillNextStart = GetTimeTillNextSkinStartDate(skin, startAdjustedWithOffset, now);
					var maxDelay = TimeSpan.FromMinutes(MaxDelayInMinutes);
					return timeTillNextStart > maxDelay ? maxDelay : timeTillNextStart;
				}

				return TimeLeftInCurrentIteration(imageDuration, startAdjustedWithOffset);
			}

			return imageDuration;
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
						yield break;
					}

					yield return skin.Images[index];
				}
				else
				{
					yield return image;
				}
			}
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

		private static TimeSpan GetTimeTillNextSkinStartDate(Skin skin, DateTime startAdjustedWithOffset, DateTime now)
		{
			if (skin.StartOffset.Hour.HasValue && !skin.StartOffset.Day.HasValue)
			{
				startAdjustedWithOffset = startAdjustedWithOffset.AddDays(1);
			}

			if (skin.StartOffset.Day.HasValue && !skin.StartOffset.Month.HasValue)
			{
				startAdjustedWithOffset = startAdjustedWithOffset.AddMonths(1);
			}

			if (skin.StartOffset.Month.HasValue)
			{
				startAdjustedWithOffset = startAdjustedWithOffset.AddYears(1);
			}

			var timeTillNextStart = startAdjustedWithOffset - now;
			return timeTillNextStart;
		}

		private static bool CheckSkinHasFinishedPlaying(Skin skin, DateTime now, DateTime startAdjustedWithOffset)
		{
			return now >= startAdjustedWithOffset.Add(skin.Duration);
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