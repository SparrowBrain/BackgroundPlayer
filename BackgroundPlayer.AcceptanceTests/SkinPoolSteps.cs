using System;
using TechTalk.SpecFlow;

namespace BackgroundPlayer.AcceptanceTests
{
    [Binding]
    public class SkinPoolSteps
    {
        [Given(@"the app started")]
        public void GivenTheAppStarted()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I press the Settings button")]
        public void WhenIPressTheSettingsButton()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see all skins listed with names")]
        public void ThenIShouldSeeAllSkinsListedWithNames()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
