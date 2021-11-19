using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Model;

namespace BackgroundPlayer.Playback
{
    public interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);

        Task PlaySkin(Skin skin, DateTime playbackStarted, CancellationToken cancellationToken);
    }
}