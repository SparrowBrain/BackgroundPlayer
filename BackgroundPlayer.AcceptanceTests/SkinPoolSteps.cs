using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Automation;
using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.InputDevices;

namespace BackgroundPlayer.AcceptanceTests
{
    [Binding]
    public class SkinPoolSteps
    {
        Application _application;

        [Given(@"the app started")]
        public void GivenTheAppStarted()
        {
            var startInfo = new ProcessStartInfo(Path.GetFullPath(@"..\..\..\BackgroundPlayer.Wpf\bin\Debug\netcoreapp3.0\BackgroundPlayer.Wpf.exe"));
            startInfo.WorkingDirectory = Path.GetFullPath(@"..\..\..\BackgroundPlayer.Wpf\bin\Debug\netcoreapp3.0\");
            _application = Application.Launch(startInfo);
            Task.Delay(5000).Wait();
            //var windows = _application.GetWindow("Settings");

            var systemTrayHandle = what.GetSystemTrayHandle();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.MainWindowHandle == systemTrayHandle)
                {
                    // ...
                    var aaaa = 0;
                }
            }

            //foreach (var icon in EnumNotificationIcons())
            //{
            //    var name = icon.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
            //    if (name.StartsWith("BackgroundPlayer"))
            //    {
            //        icon.InvokeButton();

            //        var point = icon.GetClickablePoint();
            //        Mouse.Instance.Click(MouseButton.Left, point);
            //        Task.Delay(2000).Wait();

            //        foreach (var child in icon.FindAll(TreeScope.Children,
            //  new PropertyCondition(AutomationElement.ControlTypeProperty,
            //                        ControlType.MenuItem)).Cast<AutomationElement>())
            //        {
            //            child.InvokeButton();
            //        }

            //        break;
            //    }
            //}

            _application.Close();
        }

        [When(@"I press the Settings button")]
        public void WhenIPressTheSettingsButton()
        {
        }

        [Then(@"I should see all skins listed with names")]
        public void ThenIShouldSeeAllSkinsListedWithNames()
        {
        }

        public static IEnumerable<AutomationElement> EnumNotificationIcons()
        {
            var userArea = AutomationElement.RootElement.Find(
                            "User Promoted Notification Area");
            if (userArea != null)
            {
                foreach (var button in userArea.EnumChildButtons())
                {
                    yield return button;
                }

                foreach (var button in userArea.GetTopLevelElement().Find(
                              "System Promoted Notification Area").EnumChildButtons())
                {
                    yield return button;
                }
            }

            var chevron = AutomationElement.RootElement.Find("Notification Chevron");
            if (chevron != null && chevron.InvokeButton())
            {
                foreach (var button in AutomationElement.RootElement.Find(
                                   "Overflow Notification Area").EnumChildButtons())
                {
                    yield return button;
                }
            }
        }
    }

    internal static class AutomationElementHelpers
    {
        public static AutomationElement
 Find(this AutomationElement root, string name)
        {
            return root.FindFirst(
             TreeScope.Descendants,
             new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public static IEnumerable<AutomationElement>
EnumChildButtons(this AutomationElement parent)
        {
            return parent == null ? Enumerable.Empty<AutomationElement>()
                                  : parent.FindAll(TreeScope.Children,
              new PropertyCondition(AutomationElement.ControlTypeProperty,
                                    ControlType.Button)).Cast<AutomationElement>();
        }

        public static bool
InvokeButton(this AutomationElement button)
        {
            var invokePattern = button.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;

            //button.SetFocus();
            //Keyboard.Instance.PressSpecialKey(TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys.SPACE);


            //var point = button.GetClickablePoint();
            //Mouse.Instance.Click(MouseButton.Left, point);
            if (invokePattern != null)
            {
                invokePattern.Invoke();
            }
            return invokePattern != null;
        }

        static public AutomationElement
 GetTopLevelElement(this AutomationElement element)
        {
            AutomationElement parent;
            while ((parent = TreeWalker.ControlViewWalker.GetParent(element)) !=
                 AutomationElement.RootElement)
            {
                element = parent;
            }
            return element;
        }


    }

    static class what
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpClassName, string lpWindowName);



        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);



       public  static IntPtr GetSystemTrayHandle()
        {
            IntPtr hWndTray = FindWindow("Shell_TrayWnd", null);
            if (hWndTray != IntPtr.Zero)
            {
                hWndTray = FindWindowEx(hWndTray, IntPtr.Zero, "TrayNotifyWnd", null);
                if (hWndTray != IntPtr.Zero)
                {
                    hWndTray = FindWindowEx(hWndTray, IntPtr.Zero, "SysPager", null);
                    if (hWndTray != IntPtr.Zero)
                    {
                        hWndTray = FindWindowEx(hWndTray, IntPtr.Zero, "ToolbarWindow32", null);
                        return hWndTray;
                    }
                }
            }

            return IntPtr.Zero;
        }
    }
}