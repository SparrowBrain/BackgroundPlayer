using Stylet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace BackgroundPlayer.Wpf
{
    public class RootViewModel : Conductor<Screen>
    {
        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
