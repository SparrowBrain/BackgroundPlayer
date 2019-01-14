using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Model;

namespace BackgroundPlayer
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

        public async Task PlaySkin(Skin skin, CancellationToken cancellationToken)
        {
            var start = _dateTimeProvider.Now();
            foreach (var image in _skinCalculator.NextImage(skin, start))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(image))
                {
                    return;
                }

                _windowsBackground.Refresh(image);

                await _pacer.Delay(_skinCalculator.NextDelay(skin, start));
            }
        }
    }
}