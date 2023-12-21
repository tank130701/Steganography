namespace Steganography.Service.Encoder
{
    public interface IImageEncoder
    {
        void EncodeText(string imagePath, string outputImagePath, string message, string algorithm);
        public List<string> GetImageFilesInDirectory();
    }
}
