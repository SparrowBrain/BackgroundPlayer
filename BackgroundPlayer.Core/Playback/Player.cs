using BackgroundPlayer.Core.Infrastructure;
using BackgroundPlayer.Core.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BackgroundPlayer.Core.Playback
{
	public class Player : IPlayer
	{
		private readonly ISkinCalculator _skinCalculator;
		private readonly IWindowsBackground _windowsBackground;
		private readonly IPacer _pacer;
		private readonly IDateTimeProvider _dateTimeProvider;

		public Player(ISkinCalculator skinCalculator, IWindowsBackground windowsBackground, IPacer pacer, IDateTimeProvider dateTimeProvider)
		{
			_skinCalculator = skinCalculator;
			_windowsBackground = windowsBackground;
			_pacer = pacer;
			_dateTimeProvider = dateTimeProvider;
		}

		public Task PlaySkin(Skin skin, CancellationToken cancellationToken)
		{
			var start = _dateTimeProvider.Now();
			return PlaySkin(skin, start, cancellationToken);
		}

		private async Task PlaySkin(Skin skin, DateTime playbackStarted, CancellationToken cancellationToken)
		{
			foreach (var image in _skinCalculator.NextImage(skin, playbackStarted))
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (string.IsNullOrWhiteSpace(image))
				{
					return;
				}

				_windowsBackground.Refresh(image);

				await _pacer.Delay(_skinCalculator.NextDelay(skin, playbackStarted));
			}

			await DisplayLastImage(skin, playbackStarted);
		}

		private async Task DisplayLastImage(Skin skin, DateTime playbackStarted)
		{
			var lastImage = skin.Images.Last();
			_windowsBackground.Refresh(lastImage);
			await _pacer.Delay(_skinCalculator.NextDelay(skin, playbackStarted));
		}
	}
}