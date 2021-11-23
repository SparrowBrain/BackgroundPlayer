using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using BackgroundPlayer.Core.Infrastructure;
using BackgroundPlayer.Core.Model;
using BackgroundPlayer.Core.Playback;
using Moq;
using Xunit;

namespace BackgroundPlayer.UnitTests.Playback
{
    public class Player_Should
    {
        [Theory]
        [AutoMoqData]
        public async Task ThrowCancelledException_WhenCancellationToken([Frozen] Mock<ISkinCalculator> skinCalculatorMock, Player player, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            skinCalculatorMock.Setup(x => x.NextImage(It.IsAny<Skin>(), It.IsAny<DateTime>())).Returns(skin.Images);
            cancellationTokenSource.Cancel();

            Task Act() => player.PlaySkin(skin, cancellationTokenSource.Token);

            await Assert.ThrowsAsync<OperationCanceledException>(Act);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextImageFromSkinCalculator([Frozen] Mock<ISkinCalculator> skinCalculatorMock, IEnumerable<string> imagesToPlay, [Frozen] Mock<IWindowsBackground> windowsBackgroundMock, Player player, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            skinCalculatorMock.Setup(x => x.NextImage(It.IsAny<Skin>(), It.IsAny<DateTime>())).Returns(imagesToPlay);

            await player.PlaySkin(skin, cancellationTokenSource.Token);

            foreach (var image in imagesToPlay)
            {
                windowsBackgroundMock.Verify(x => x.Refresh(image));
            }

        }

        [Theory]
        [AutoMoqData]
        public async Task StopPlayingWhenTheresNoNextImage([Frozen] Mock<IWindowsBackground> windowsBackgroundMock, Player player, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            await player.PlaySkin(skin, cancellationTokenSource.Token);

            windowsBackgroundMock.Verify(x => x.Refresh(It.IsAny<string>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task GetDelayFromSkinCalculator([Frozen] Mock<ISkinCalculator> skinCalculatorMock, [Frozen] Mock<IPacer> pacerMock, Player player, Skin skin, CancellationTokenSource cancellationTokenSource)
        {
            skinCalculatorMock.Setup(x => x.NextImage(It.IsAny<Skin>(), It.IsAny<DateTime>())).Returns(skin.Images);

            await player.PlaySkin(skin, cancellationTokenSource.Token);
            
            pacerMock.Verify(x => x.Delay(skinCalculatorMock.Object.NextDelay(skin, It.IsAny<DateTime>())));
        }
    }
}