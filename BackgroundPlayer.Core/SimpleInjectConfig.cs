using BackgroundPlayer.Core.Configuration;
using BackgroundPlayer.Core.Infrastructure;
using BackgroundPlayer.Core.Playback;
using SimpleInjector;

namespace BackgroundPlayer.Core
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

            container.Register<ISkinValidator, SkinValidator>();
            container.Register<SkinLoader>();

            container.Verify();
            return container;
        }
    }
}