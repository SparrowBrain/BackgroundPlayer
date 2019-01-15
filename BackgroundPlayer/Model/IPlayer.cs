using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlayer.Model
{
    internal interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);
    }
}