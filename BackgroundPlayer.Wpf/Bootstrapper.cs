using BackgroundPlayer.Configuration;
using BackgroundPlayer.Infrastructure;
using BackgroundPlayer.Model;
using Stylet;
using StyletIoC;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace BackgroundPlayer.Wpf
{
    internal class Bootstrapper : Bootstrapper<RootViewModel>
    {
        protected override void OnStart()
        {
            // This is called just after the application is started, but before the IoC container is set up.
            // Set up things like logging, etc
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Bind your own types. Concrete types are automatically self-bound.
            builder.Autobind(Assembly.GetAssembly(typeof(SkinLoader)));
            builder.Bind<ISkinValidator>().To<SkinValidator>().InSingletonScope();
            builder.Bind<IPlayer>().To<Player>().InSingletonScope();

            builder.Bind<ISkinCalculator>().To<SkinCalculator>().InSingletonScope();
            builder.Bind<IWindowsBackground>().To<WindowsBackground>().InSingletonScope();
            builder.Bind<IDateTimeProvider>().To<DateTimeProvider>().InSingletonScope();
            builder.Bind<IPacer>().To<Pacer>().InSingletonScope();
        }

        protected override void Configure()
        {
            // This is called after Stylet has created the IoC container, so this.Container exists, but before the
            // Root ViewModel is launched.
            // Configure your services, etc, in here
        }

        protected override void OnLaunch()
        {
            // This is called just after the root ViewModel has been launched
            // Something like a version check that displays a dialog might be launched from here
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Called on Application.Exit
        }

        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            // Called on Application.DispatcherUnhandledException
        }
    }
}