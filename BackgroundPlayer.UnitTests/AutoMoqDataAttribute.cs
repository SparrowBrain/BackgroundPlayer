using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace BackgroundPlayer.UnitTests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects)
        {
        }
    }
}