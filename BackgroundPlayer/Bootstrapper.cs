using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using BackgroundPlayer.Core;
using BackgroundPlayer.Core.Configuration;
using BackgroundPlayer.Core.Infrastructure;
using BackgroundPlayer.Core.Playback;
using Caliburn.Micro;

namespace BackgroundPlayer;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container;

    public Bootstrapper()
    {
        Initialize();
    }

    protected override void Configure()
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        _container = new SimpleContainer();

        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();

        _container.PerRequest<RootViewModel>();
        _container.PerRequest<SkinPoolViewModel>();

        _container.Singleton<StartUp>();
        _container.Singleton<SkinLoader>();
        _container.Singleton<PlaylistPlayer>();
        _container.Singleton<ISkinValidator, SkinValidator>();
        _container.Singleton<IPlayer, Player>();

        _container.Singleton<ISkinCalculator, SkinCalculator>();
        _container.Singleton<IWindowsBackground, WindowsBackground>();
        _container.Singleton<IDateTimeProvider, DateTimeProvider>();
        _container.Singleton<IPacer, Pacer>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewFor<RootViewModel>();

        var startUp = _container.GetInstance<StartUp>();
        var playlistPlayer = _container.GetInstance<PlaylistPlayer>();
        var cancellationTokenSource = new CancellationTokenSource();
        
        var skins = startUp.LoadSkins();

        _ = playlistPlayer.Play(skins, cancellationTokenSource.Token);
    }

    protected override IEnumerable<Assembly> SelectAssemblies()
    {
        return new[] { Assembly.GetExecutingAssembly() };
    }
}