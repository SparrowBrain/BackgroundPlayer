using BackgroundPlayer.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlayer.Core.Playback
{
    public class PlaylistPlayer
    {
        private readonly IPlayer _player;

        public PlaylistPlayer(IPlayer player)
        {
            _player = player;
        }

        public async Task Play(List<Skin> skins, CancellationToken cancellationToken)
        {
            var random = new Random();
            var skin = skins[random.Next(skins.Count)];
            while (true)
            {
                await _player.PlaySkin(skin, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}