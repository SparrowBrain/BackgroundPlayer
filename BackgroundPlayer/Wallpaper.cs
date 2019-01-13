using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlayer
{
    internal class Wallpaper
    {
        public static async Task RunWallpaperVideo()
        {
            var images = new List<string>
            {
                //@"C:\Users\Qwx\Pictures\Planetside2\screenshot_20131129-21-51-45.jpg",
                //@"C:\Users\Qwx\Pictures\Planetside2\screenshot_20131229-13-41-22.jpg"
            };

            images.AddRange(Directory.EnumerateFiles(@"C:\Users\Qwx\Desktop\Mars"));
            //images.AddRange(Directory.EnumerateFiles(@"C:\Users\Public\Pictures\Wallpapers\Gaming"));

            while (true)
            {
                foreach (var image in images)
                {
                    UpdateImage.Refresh(image);

                    await Task.Delay(200);
                }
            }
        }

        private class UpdateImage
        {
            private static readonly int SPI_SETDESKWALLPAPER = 0x14;
            private static readonly int SPIF_UPDATEINIFILE = 0x01;
            private static readonly int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            public static void Refresh(string path)
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE /*| SPIF_SENDWININICHANGE*/);
            }
        }
    }

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

    internal interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);
    }

    public interface IWindowsBackground
    {
        void Refresh(string path);
    }

    public interface IPacer
    {
        Task Delay(TimeSpan time);
    }

    public class Skin
    {
        public Skin(IList<string> images, TimeSpan duration, StartOffset startOffset)
        {
            Images = images;
            Duration = duration;
            StartOffset = startOffset;
        }

        public IList<string> Images { get; }
        public TimeSpan Duration { get; }
        public StartOffset StartOffset { get; }
    }

    public class StartOffset
    {
        public int? Month { get; set; }
        public int? Day { get; set; }

        public int? Hour { get; set; }
    }

    public interface ISkinCalculator
    {
        TimeSpan NextDelay(Skin skin, DateTime playbackStart);

        IEnumerable<string> NextImage(Skin skin, DateTime playbackStart);
    }

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
            if (skin.StartOffset.Hour.HasValue)
            {
                var startAdjustedWithOffset = new DateTime(playbackStart.Year,
                    playbackStart.Month,
                    playbackStart.Day,
                    skin.StartOffset.Hour.Value,
                    00,
                    00);

                var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
                return imageDuration - TimeSpan.FromMilliseconds(timeSinceStart.TotalMilliseconds % imageDuration.TotalMilliseconds);
            }

            return imageDuration;
        }

        public IEnumerable<string> NextImage(Skin skin, DateTime playbackStart)
        {
            if (skin.StartOffset.Hour.HasValue)
            {
                var imageDuration = skin.Duration / skin.Images.Count;
                var startAdjustedWithOffset = new DateTime(playbackStart.Year,
                    playbackStart.Month,
                    playbackStart.Day,
                    skin.StartOffset.Hour.Value,
                    00,
                    00);

                var timeSinceStart = _dateTimeProvider.Now() - startAdjustedWithOffset;
                var index = (int)(timeSinceStart.TotalMilliseconds / imageDuration.TotalMilliseconds);
                yield return skin.Images[index];
            }

            foreach (var image in skin.Images)
            {
                yield return image;
            }
        }
    }

    public interface IDateTimeProvider
    {
        DateTime Now();
    }

    internal class SkinLoader
    {
        private readonly Configuration _configuration;

        public SkinLoader(Configuration configuration)
        {
            _configuration = configuration;
        }

        public List<Skin> LoadSkins()
        {
            return new List<Skin>();
        }
    }

    internal class Configuration
    {
        public string SkinsPath { get; set; } = ".\\skins";
    }
}