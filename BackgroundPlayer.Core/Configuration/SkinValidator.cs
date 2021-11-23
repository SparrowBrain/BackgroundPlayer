using System.IO;
using System.Linq;

namespace BackgroundPlayer.Core.Configuration
{
    public class SkinValidator : ISkinValidator
    {
        public bool ValidImageExtension(string imagePath)
        {
            var fileExtension = Path.GetExtension(imagePath).ToLowerInvariant();
            return new[] { ".png", ".jpg", ".jpeg", ".bmp" }.Contains(fileExtension);
        }
    }
}