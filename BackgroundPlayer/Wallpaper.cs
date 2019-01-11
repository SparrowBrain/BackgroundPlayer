using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BackgroundPlayer
{
    internal class Wallpaper
    {
        public static async Task RunWallpaperVideo()
        {
            var images = new List<string>
            {
                //@"C:\Users\Qwx\Pictures\Planetside2\screenshot_20131129-21-51-45.jpg",
                //@"C:\Users\Qwx\Pictures\Planetside2\screenshot_20131229-13-41-22.jpg"
            };

            images.AddRange(Directory.EnumerateFiles(@"C:\Users\Qwx\Desktop\Mars"));
            //images.AddRange(Directory.EnumerateFiles(@"C:\Users\Public\Pictures\Wallpapers\Gaming"));

            while (true)
            {
                foreach (var image in images)
                {
                    UpdateImage.Refresh(image);

                    await Task.Delay(42);
                }
            }
        }

        private class UpdateImage
        {
            private static readonly int SPI_SETDESKWALLPAPER = 0x14;
            private static readonly int SPIF_UPDATEINIFILE = 0x01;
            private static readonly int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            public static void Refresh(string path)
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
    }
}