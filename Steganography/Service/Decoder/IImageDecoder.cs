namespace Steganography.Service.Decoder
{
    public interface IImageDecoder
    {
        string DecodeMessage(string imagePath, string algorithm);
        public List<string> GetImageFilesInDirectory();
    }
}