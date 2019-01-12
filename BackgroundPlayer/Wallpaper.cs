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

        public Player(ISkinCalculator skinCalculator, IWindowsBackground windowsBackground, IPacer pacer)
        {
            _skinCalculator = skinCalculator;
            _windowsBackground = windowsBackground;
            _pacer = pacer;
        }

        public async Task PlaySkin(Skin skin, CancellationToken cancellationToken)
        {
            foreach (var image in _skinCalculator.NextImage(skin))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(image))
                {
                    return;
                }

                _windowsBackground.Refresh(image);

                await _pacer.Delay(_skinCalculator.NextDelay(skin));
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
        public Skin(IList<string> images, TimeSpan duration)
        {
            Images = images;
            Duration = duration;
        }

        public IList<string> Images { get; private set; }
        public TimeSpan Duration { get; private set; }
    }

    public interface ISkinCalculator
    {
        TimeSpan NextDelay(Skin skin);
        IEnumerable<string> NextImage(Skin skin);
    }

    public class SkinCalculator : ISkinCalculator
    {
        public SkinCalculator()
        {
            
        }

        public TimeSpan NextDelay(Skin skin)
        {
            var delay = skin.Duration / skin.Images.Count;
            return delay;
        }

        public IEnumerable<string> NextImage(Skin skin)
        {
            throw new NotImplementedException();
        }
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