namespace Steganography.Service.Encoder
{
    public interface IImageEncoder
    {
        void EncodeMessage(string imagePath, string message, string algorithm);
        public List<string> GetImageFilesInDirectory();
    }
}
