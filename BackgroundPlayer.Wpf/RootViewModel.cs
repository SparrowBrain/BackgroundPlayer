using Caliburn.Micro;
using System.Threading.Tasks;
using System.Windows;

namespace BackgroundPlayer.Wpf
{
    public class RootViewModel : Conductor<SkinPoolViewModel>
    {
        private readonly IWindowManager _windowManager;
        private bool _extendedMenu;

        public RootViewModel(IWindowManager windowManager, SkinPoolViewModel skinPoolViewModel)
        {
            _windowManager = windowManager;
            ActiveItem = skinPoolViewModel;
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

        public string Icon => "/Icons/BackgroundPlayer.ico";

        public async Task ShowSettings()
        {
            await _windowManager.ShowWindowAsync(ActiveItem);
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
    }
}