using BackgroundPlayer.Configuration;
using BackgroundPlayer.Model;
using SimpleInjector;
using System;
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
            Container = new SimpleInjectConfig().Setup();
        }

        public static async Task Main(string[] args)
        {
            var random = new Random();
            var skinLoader = Container.GetInstance<SkinLoader>();
            var skins = skinLoader.LoadSkins();

            var player = Container.GetInstance<IPlayer>();

            var cancellationTokenSource = new CancellationTokenSource();

            while (true)
            {
                var skin = skins[random.Next(skins.Count)];
                await player.PlaySkin(skin, cancellationTokenSource.Token);
            }
        }
    }
}