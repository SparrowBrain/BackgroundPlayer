using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BackgroundPlayer
{
    internal class LockScreen
    {
        private const string Backgroud = @"C:\Windows\Web\Wallpaper\W10DBSI.png";

        public static async Task Rotate()
        {
            var images = new List<string>
            {
                @"C:\hacks\wallpaper\orange.png",
                @"C:\hacks\wallpaper\W10DBSI.png",
                //@"C:\hacks\wallpaper\Attention.png"
            };

            while (true)
            {
                foreach (var image in images)
                {
                    try
                    {
                        File.Copy(image, Backgroud, true);
                        Console.WriteLine(image);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    await Task.Delay(5000);
                }
            }
        }
    }
}