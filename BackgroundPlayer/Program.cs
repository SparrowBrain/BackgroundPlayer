using BackgroundPlayer.Configuration;
using BackgroundPlayer.Model;
using SimpleInjector;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BackgroundPlayer.UnitTests")]
[assembly: InternalsVisibleTo("BackgroundPlayer.IntegrationTests")]

namespace BackgroundPlayer
{
    internal class Program
    {
        private static readonly Container Container;

        static Program()
        {
            Container = new Registry.Registry().Setup();
        }

        public static async Task Main(string[] args)
        {
            var skinLoader = Container.GetInstance<SkinLoader>();
            var skins = skinLoader.LoadSkins();

            var player = Container.GetInstance<IPlayer>();

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