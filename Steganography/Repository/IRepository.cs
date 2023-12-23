using System.Drawing;

namespace Steganography.Repository
{
    public interface IRepository
    {
  
        Bitmap LoadImageToBitmap(string imagePath);
        string SaveImageFromBitmap(Bitmap image);
        byte[] LoadImageToBytes(string imageFolder, string imagePath);
        string SaveImageFromBytes(byte[] image);
        List<string> GetImageFilesInDirectory(string directoryPath);
    }
}
