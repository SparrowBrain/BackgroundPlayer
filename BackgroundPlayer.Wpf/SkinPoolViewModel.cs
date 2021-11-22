using System.Linq;
using Caliburn.Micro;

namespace BackgroundPlayer.Wpf
{
    public class SkinPoolViewModel : Screen
    {
        public SkinPoolViewModel()
        {
            Skins = new BindableCollection<SkinDetailsViewModel>();
        }

        public BindableCollection<SkinDetailsViewModel> Skins { get; set; }
    }
}