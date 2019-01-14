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

                    await Task.Delay(200);
                }
            }
        }

        private class UpdateImage
        {
            public static void Refresh(string image)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}