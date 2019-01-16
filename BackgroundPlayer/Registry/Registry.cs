using BackgroundPlayer.Configuration;
using BackgroundPlayer.Infrastructure;
using BackgroundPlayer.Model;
using SimpleInjector;

namespace BackgroundPlayer.Registry
{
    internal class Registry
    {
        public Container Setup()
        {
            var container = new Container();

            container.Register<IDateTimeProvider, DateTimeProvider>(Lifestyle.Singleton);
            container.Register<IPacer, Pacer>(Lifestyle.Singleton);
            container.Register<IWindowsBackground, WindowsBackground>(Lifestyle.Singleton);

            container.Register<IPlayer, Player>(Lifestyle.Singleton);
            container.Register<ISkinCalculator, SkinCalculator>(Lifestyle.Singleton);

            container.Register<Configuration.Configuration>();
            container.Register<SkinLoader>();

            container.Verify();
            return container;
        }
    }
}