using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BackgroundPlayer.UnitTests")]
namespace BackgroundPlayer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await BackgroundPlayer.Wallpaper.RunWallpaperVideo();
            //await LockScreen.Rotate();
        }
    }
}