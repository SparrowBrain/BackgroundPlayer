using AutoFixture;
using BackgroundPlayer.Configuration;
using BackgroundPlayer.Infrastructure;
using BackgroundPlayer.Model;
using BackgroundPlayer.Wpf;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace BackgroundPlayer.AcceptanceTests
{
    [Binding]
    public class SkinPoolSteps
    {
        private Fixture _fixture;
        private RootViewModel _rootViewModel;

        public SkinPoolSteps()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(ISkinValidator), typeof(SkinValidator)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(IPlayer), typeof(Player)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(ISkinCalculator), typeof(SkinCalculator)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(IDateTimeProvider), typeof(DateTimeProvider)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(IPacer), typeof(Pacer)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(IWindowsBackground), typeof(WindowsBackground)));
            _fixture.Customizations.Add(new AutoFixture.Kernel.TypeRelay(typeof(ISkinValidator), typeof(SkinValidator)));

            var settings = _fixture.Freeze<Settings>();

            var skinName = _fixture.Create<string>();
            var SkinConfig = _fixture.Create<SkinConfig>();
            var SkinsPath = settings.SkinsPath;
            var skinJson = JsonConvert.SerializeObject(SkinConfig);
            var skinPath = Path.Combine(SkinsPath, skinName);
            Directory.CreateDirectory(skinPath);
            File.WriteAllText(Path.Combine(skinPath, "skin.json"), skinJson);
            var imagePath = Path.Combine(skinPath, "images");
            Directory.CreateDirectory(imagePath);

            var ImageFiles = _fixture.CreateMany<string>().Select(x => Path.Combine(imagePath, x + ".jpg")).OrderBy(x => x);
            foreach (var image in ImageFiles)
            {
                File.WriteAllText(image, string.Empty);
            }
        }

        [Given(@"the app started")]
        public void GivenTheAppStarted()
        {
            var app = new App();
        }

        [When(@"I press the Settings button")]
        public void WhenIPressTheSettingsButton()
        {
            _rootViewModel.ShowSettings = true;
        }

        [Then(@"I should see all skins listed with names")]
        public void ThenIShouldSeeAllSkinsListedWithNames()
        {
            Assert.Single(_rootViewModel.ActiveItem.Skins);
        }
    }
}