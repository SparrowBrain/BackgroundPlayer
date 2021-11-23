using SimpleInjector;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Playback;

[assembly: InternalsVisibleTo("BackgroundPlayer.UnitTests")]
[assembly: InternalsVisibleTo("BackgroundPlayer.IntegrationTests")]

namespace BackgroundPlayer
{
    internal class Program
    {
        private static readonly Container Container;

        static Program()
        {
            Container = new SimpleInjectConfig().Setup();
        }

        public static async Task Main(string[] args)
        {
            var startUp = Container.GetInstance<StartUp>();
            var playlistPlayer = Container.GetInstance<PlaylistPlayer>();

            var skins = startUp.LoadSkins();
            var cancellationTokenSource = new CancellationTokenSource();

            await playlistPlayer.Play(skins, cancellationTokenSource.Token);
        }
    }
}