using BackgroundPlayer.Playback;
using Stylet;
using System.Threading;
using System.Windows;

namespace BackgroundPlayer.Wpf
{
    public class RootViewModel : Conductor<SkinPoolViewModel>
    {
        private readonly IWindowManager _windowManager;
        private readonly StartUp _startUp;
        private readonly PlaylistPlayer _playlistPlayer;

        public RootViewModel(IWindowManager windowManager, StartUp startUp, PlaylistPlayer playlistPlayer)
        {
            _windowManager = windowManager;
            _startUp = startUp;
            _playlistPlayer = playlistPlayer;
        }
        
        public void ShowSettings()
        {
            _windowManager.ShowWindow(ActiveItem);
        }
        
        public void Exit()
        {
            Application.Current.Shutdown();
        }

        protected override void OnInitialActivate()
        {
            var skins = _startUp.LoadSkins();
            ActiveItem = new SkinPoolViewModel(skins);

            var cancellationTokenSource = new CancellationTokenSource();
            _playlistPlayer.Play(skins, cancellationTokenSource.Token);
        }
    }
}