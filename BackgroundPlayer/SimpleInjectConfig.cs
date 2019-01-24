using BackgroundPlayer.Configuration;
using BackgroundPlayer.Infrastructure;
using BackgroundPlayer.Model;
using BackgroundPlayer.Playback;
using SimpleInjector;

namespace BackgroundPlayer
{
    internal class SimpleInjectConfig
    {
        public Container Setup()
        {
            var container = new Container();

            container.Register<IDateTimeProvider, DateTimeProvider>(Lifestyle.Singleton);
            container.Register<IPacer, Pacer>(Lifestyle.Singleton);
            container.Register<IWindowsBackground, WindowsBackground>(Lifestyle.Singleton);

            container.Register<IPlayer, Player>(Lifestyle.Singleton);
            container.Register<ISkinCalculator, SkinCalculator>(Lifestyle.Singleton);

            container.Register<Configuration.Settings>();
            container.Register<ISkinValidator, SkinValidator>();
            container.Register<SkinLoader>();

            container.Verify();
            return container;
        }
    }
}