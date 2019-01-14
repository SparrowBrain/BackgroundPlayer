using System.Threading;
using System.Threading.Tasks;
using BackgroundPlayer.Model;

namespace BackgroundPlayer
{
    internal interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);
    }
}