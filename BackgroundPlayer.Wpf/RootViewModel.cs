using Stylet;
using System.Threading;
using System.Windows;

namespace BackgroundPlayer.Wpf
{
    public class RootViewModel : Conductor<SkinPoolViewModel>
    {
        private readonly StartUp startUp;
        private readonly PlaylistPlayer playlistPlayer;

        public RootViewModel(StartUp startUp, PlaylistPlayer playlistPlayer)
        {
            this.startUp = startUp;
            this.playlistPlayer = playlistPlayer;
        }

        public bool ShowSettings { get; set; }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        protected override async void OnInitialActivate()
        {
            var skins = startUp.LoadSkins();
            ActiveItem = new SkinPoolViewModel(skins);

            var cancellationTokenSource = new CancellationTokenSource();
            await playlistPlayer.Play(skins, cancellationTokenSource.Token);

            
        }
    }
}