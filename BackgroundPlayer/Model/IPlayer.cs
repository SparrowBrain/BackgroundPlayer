using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlayer.Model
{
    public interface IPlayer
    {
        Task PlaySkin(Skin skin, CancellationToken cancellationToken);
    }
}