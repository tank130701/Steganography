namespace Steganography.Service.Encoder
{
    public interface IImageEncoder
    {
        string EncodeMessage(string imagePath, string message, string algorithm);
        public List<string> GetImageFilesInDirectory();
    }
}
