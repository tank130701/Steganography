namespace Steganography.Service.Decoder
{
    internal interface IImageDecoder
    {
        void DecodeText(string imagePath);
    }
}