namespace Steganography.Service.Decoder
{
    public interface IImageDecoder
    {
        void DecodeText(string imagePath);
    }
}