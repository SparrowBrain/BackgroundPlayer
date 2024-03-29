﻿using System.IO;
using System.Runtime.InteropServices;

namespace BackgroundPlayer.Core.Infrastructure
{
    public class WindowsBackground : IWindowsBackground
    {
        private static readonly int SPI_SETDESKWALLPAPER = 0x14;
        private static readonly int SPIF_UPDATEINIFILE = 0x01;
        private static readonly int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public void Refresh(string path)
        {
            var fullPath = Path.GetFullPath(path);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fullPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}