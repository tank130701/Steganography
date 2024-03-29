using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Steganography.Repository
{
    public interface IRepository
    {
        Image<Rgba32> LoadImageToRGB(string imageFolder, string imageName);
        string SaveImageFromRGB(Image<Rgba32> image, string imageType);
        byte[] LoadImageToBytes(string imageFolder, string imagePath);
        string SaveImageFromBytes(byte[] image, string imageType);
        List<string> GetImageFilesInDirectory(string directoryPath);
    }
}
