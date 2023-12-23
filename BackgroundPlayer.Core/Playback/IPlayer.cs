using BackgroundPlayer.Core.Model;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundPlayer.Core.Playback
{
	public interface IPlayer
	{
		Task PlaySkin(Skin skin, CancellationToken cancellationToken);
	}
}