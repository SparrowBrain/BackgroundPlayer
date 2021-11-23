using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Core.Configuration;
using BackgroundPlayer.Core.Model;

namespace BackgroundPlayer.Core.Playback
{
    public class PlaylistPlayer
    {
        private readonly IPlayer _player;
        private readonly PlaybackState _playbackState;

        public PlaylistPlayer(IPlayer player, PlaybackState playbackState)
        {
            _player = player;
            _playbackState = playbackState;
        }

        public async Task Play(List<Skin> skins, CancellationToken cancellationToken)
        {
            if(_playbackState!=null)
            {
                var skin = skins.FirstOrDefault(x => x.Name == _playbackState.SkinName);
                if (skin != null)
                {
                    await _player.PlaySkin(skin, _playbackState.PlaybackStarted, cancellationToken);
                }
            }
            var random = new Random();
            while (true)
            {
                var skin = skins[random.Next(skins.Count)];
                await _player.PlaySkin(skin, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}