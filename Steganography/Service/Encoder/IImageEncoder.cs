namespace Steganography.Service.Encoder
{
    public interface IImageEncoder
    {
        void EncodeText(string imagePath, string outputImagePath, string text, string algorithm);
        public List<string> GetImageFilesInDirectory();
    }
}
