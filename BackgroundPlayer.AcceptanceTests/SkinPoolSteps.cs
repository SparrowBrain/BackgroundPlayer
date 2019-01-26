using AutoFixture;
using BackgroundPlayer.Configuration;
using BackgroundPlayer.Infrastructure;
using BackgroundPlayer.Model;
using BackgroundPlayer.Wpf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Automation;
using TechTalk.SpecFlow;
using TestStack.White;
using Xunit;

namespace BackgroundPlayer.AcceptanceTests
{
    [Binding]
    public class SkinPoolSteps
    {

        public SkinPoolSteps()
        {
        }

        [Given(@"the app started")]
        public void GivenTheAppStarted()
        {
            var application = Application.Launch(@"..\..\..\..\BackgroundPlayer.Wpf\bin\Debug\netcoreapp3.0\BackgroundPlayer.Wpf.exe");
            //AutomationElement.RootElement.

            foreach (var icon in EnumNotificationIcons())
            {
                var name = icon.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                if (name.StartsWith("Background"))
                {
                    icon.InvokeButton();
                    break;
                }
            }
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

    static class AutomationElementHelpers
    {
        public static AutomationElement        Find(this AutomationElement root, string name)
        {
            return root.FindFirst(
             TreeScope.Descendants,
             new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public static IEnumerable<AutomationElement> EnumChildButtons(this AutomationElement parent)
        {
            return parent == null ? Enumerable.Empty<AutomationElement>()
                                  : parent.FindAll(TreeScope.Children,
              new PropertyCondition(AutomationElement.ControlTypeProperty,
                                    ControlType.Button)).Cast<AutomationElement>();
        }

        public static bool InvokeButton(this AutomationElement button)
        {
            var invokePattern = button.GetCurrentPattern(InvokePattern.Pattern)
                               as InvokePattern;
            if (invokePattern != null)
            {
                invokePattern.Invoke();
            }
            return invokePattern != null;
        }

        static public AutomationElement GetTopLevelElement(this AutomationElement element)
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
}

