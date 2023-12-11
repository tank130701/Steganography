namespace Steganography.Service.Encoder
{
    internal interface IImageEncoder
    {
        void EncodeText(string imagePath, string outputImagePath, string text);
    }
}
