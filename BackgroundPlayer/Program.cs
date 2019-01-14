using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BackgroundPlayer.UnitTests")]
namespace BackgroundPlayer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var skinLoader = new SkinLoader(new Configuration());
            var skins = skinLoader.LoadSkins();

            var dateTimeProvider = new DateTimeProvider();
            var skinCalculator = new SkinCalculator(dateTimeProvider);
            var windowsBackground = new WindowsBackground();
            var pacer = new Pacer();
            var player = new Player(skinCalculator, windowsBackground, pacer, dateTimeProvider);

            var cancellationTokenSource = new CancellationTokenSource();

            while (true)
            {
                foreach (var skin in skins)
                {
                    await player.PlaySkin(skin, cancellationTokenSource.Token);
                }
            }

            //await BackgroundPlayer.Wallpaper.RunWallpaperVideo();
            //await LockScreen.Rotate();
        }
    }
}