using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BackgroundPlayer.UnitTests
{
    public class Player_Should
    {
        private IFixture _fixture;

        public Player_Should()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Theory]
        [AutoData]
        public async Task ThrowCancelledException_WhenCancellationToken([Frozen] Mock<IWindowsBackground> windowsBackgroundMock, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            var player = _fixture.Create<Player>();

            Task Act() => player.PlaySkin(skin, cancellationTokenSource.Token);

            await Assert.ThrowsAsync<OperationCanceledException>(Act);
        }

        [Theory]
        [AutoData]
        public async Task SetAllImagesFromSkinInOrder([Frozen] Mock<IWindowsBackground> windowsBackgroundMock, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            var imagesSentToBackground = new List<string>();
            windowsBackgroundMock.Setup(x => x.Refresh(It.IsAny<string>())).Callback<string>(x =>
            {
                imagesSentToBackground.Add(x);
            });
            var player = _fixture.Create<Player>();

            await player.PlaySkin(skin, cancellationTokenSource.Token);

            for (var i = 0; i < skin.Images.Count(); i++)
            {
                Assert.Equal(skin.Images[i], imagesSentToBackground[i]);
            }
        }
    }
}