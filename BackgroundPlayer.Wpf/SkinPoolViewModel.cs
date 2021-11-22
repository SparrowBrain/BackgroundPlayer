using BackgroundPlayer.Configuration;
using Caliburn.Micro;
using System.Linq;

namespace BackgroundPlayer.Wpf
{
    public class SkinPoolViewModel : Conductor<SkinDetailsViewModel>.Collection.OneActive
    {
        public SkinPoolViewModel(SkinLoader skinLoader)
        {
            Items.AddRange(skinLoader.LoadSkins().Select(x => new SkinDetailsViewModel { Name = x.Name, Duration = x.Duration, OffsetMonth = x.StartOffset.Month, OffsetDay = x.StartOffset.Day, OffsetHour = x.StartOffset.Hour }));
        }
    }
}