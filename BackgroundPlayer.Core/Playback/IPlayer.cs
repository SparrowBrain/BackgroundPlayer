using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Core.Model;

namespace BackgroundPlayer.Core.Playback
{
    public interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);

        Task PlaySkin(Skin skin, DateTime playbackStarted, CancellationToken cancellationToken);
    }
}