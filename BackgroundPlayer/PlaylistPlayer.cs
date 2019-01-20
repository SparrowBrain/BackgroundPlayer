using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Model;

namespace BackgroundPlayer
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
            while (true)
            {
                var skin = skins[random.Next(skins.Count)];
                await _player.PlaySkin(skin, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}