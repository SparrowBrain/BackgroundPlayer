using System;
using AutoFixture.Xunit2;
using BackgroundPlayer.Model;
using BackgroundPlayer.Playback;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace BackgroundPlayer.UnitTests.Playback
{
    public class PlaylistPlayer_Should
    {
        [Theory, AutoMoqData]
        private async Task ThrowOperationCanceledException_WhenCancellationRequested(
            List<Skin> skins,
            CancellationTokenSource cancellationTokenSource,
            PlaylistPlayer playlistPlayer)
        {
            cancellationTokenSource.Cancel();

            var act = new Func<Task>(() => playlistPlayer.Play(skins, cancellationTokenSource.Token));

            await Assert.ThrowsAsync<OperationCanceledException>(act);
        }

        [Theory, AutoMoqData]
        private async Task PlayGivenSkin(
            [Frozen] Mock<IPlayer> playerMock,
            CancellationTokenSource cancellationTokenSource,
            PlaylistPlayer playlistPlayer)
        {
            var fixture = new Fixture();
            var skins = fixture.CreateMany<Skin>(1).ToList();
            cancellationTokenSource.Cancel();

            try
            {
                await playlistPlayer.Play(skins, cancellationTokenSource.Token);
            }
            catch(OperationCanceledException)
            { }

            playerMock.Verify(x => x.PlaySkin(It.Is<Skin>(s => s.Name == skins.First().Name), It.IsAny<CancellationToken>()));
        }
    }
}