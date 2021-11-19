using BackgroundPlayer.Playback;
using Stylet;
using System;
using System.Threading;
using System.Windows;

namespace BackgroundPlayer.Wpf
{
    public class RootViewModel : Conductor<SkinPoolViewModel>
    {
        private readonly IWindowManager _windowManager;
        private readonly StartUp _startUp;
        private readonly PlaylistPlayer _playlistPlayer;
        private bool _extendedMenu;

        public RootViewModel(IWindowManager windowManager, StartUp startUp, PlaylistPlayer playlistPlayer)
        {
            _windowManager = windowManager;
            _startUp = startUp;
            _playlistPlayer = playlistPlayer;
        }

        public bool ExtendedMenu
        {
            get => _extendedMenu;
            set
            {
                if (value == _extendedMenu) return;
                _extendedMenu = value;
                NotifyOfPropertyChange();
            }
        }

        public void ShowSettings()
        {
            _windowManager.ShowWindow(ActiveItem);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void ShowSmallTrayMenu()
        {
            ExtendedMenu = false;
        }

        public void ShowExtendedTrayMenu()
        {
            ExtendedMenu = true;
        }

        protected override void OnInitialActivate()
        {
            //var args = Environment.GetCommandLineArgs();

            var skins = _startUp.LoadSkins();
            ActiveItem = new SkinPoolViewModel(skins);
            //ShowSettings();

            var cancellationTokenSource = new CancellationTokenSource();
            _playlistPlayer.Play(skins, cancellationTokenSource.Token);
        }
    }
}