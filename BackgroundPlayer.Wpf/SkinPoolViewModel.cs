using Stylet;
using System.Linq;

namespace BackgroundPlayer.Wpf
{
    public class SkinPoolViewModel : Screen
    {
        public SkinPoolViewModel(System.Collections.Generic.List<Model.Skin> skins)
        {
            Skins = new BindableCollection<SkinDetailsViewModel>();
            Skins.AddRange(skins.Select(x => new SkinDetailsViewModel { Name = x.Name, Duration = x.Duration, OffsetMonth = x.StartOffset.Month, OffsetDay = x.StartOffset.Day, OffsetHour = x.StartOffset.Hour }));
        }

        public BindableCollection<SkinDetailsViewModel> Skins { get; set; }
    }
}