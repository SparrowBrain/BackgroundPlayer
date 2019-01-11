using System.Threading.Tasks;

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