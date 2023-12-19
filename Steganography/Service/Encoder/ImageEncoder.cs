using Steganography.Repository;
using Steganography.Service.Algorithms;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    public void EncodeMessage(string imagePath, string message, string algorithm)
    {
        switch (algorithm)
        {
            case EncodeAlgorithms.Eof:
                var image = repository.LoadImageToBytes(imagePath);
                var encodedImage = EOFWriter.WritePastEOFMarker(image, message);
                repository.SaveImageFromBytes(encodedImage);                
                break;
                
        }
    }
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}