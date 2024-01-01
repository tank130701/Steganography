using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Steganography.Repository
{
    public interface IRepository
    {
        Image<Rgba32> LoadImageToRGB(string imageFolder, string imageName);
        void SaveImageFromRGB(Image<Rgba32> image, string imageType);
        byte[] LoadImageToBytes(string imageFolder, string imagePath);
        string SaveImageFromBytes(byte[] image);
        List<string> GetImageFilesInDirectory(string directoryPath);
    }
}
