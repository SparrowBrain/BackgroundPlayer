using AutoFixture.Xunit2;

namespace BackgroundPlayer.UnitTests
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects)
        {
        }
    }
}