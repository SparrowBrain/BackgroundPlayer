using BackgroundPlayer.Configuration;
using Caliburn.Micro;
using System.Linq;

namespace BackgroundPlayer.Wpf
{
    public class SkinPoolViewModel : Screen
    {
        public SkinPoolViewModel(SkinLoader skinLoader)
        {
            Skins = new BindableCollection<SkinDetailsViewModel>();
            Skins.AddRange(skinLoader.LoadSkins().Select(x => new SkinDetailsViewModel { Name = x.Name, Duration = x.Duration, OffsetMonth = x.StartOffset.Month, OffsetDay = x.StartOffset.Day, OffsetHour = x.StartOffset.Hour }));
        }

        public BindableCollection<SkinDetailsViewModel> Skins { get; set; }
    }
}