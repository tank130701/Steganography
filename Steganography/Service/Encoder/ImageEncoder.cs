using Steganography.Repository;
using Steganography.Service.Algorithms;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private byte[] _image;
    private byte[] _encodedImage;
    public void EncodeMessage(string imagePath, string message, string algorithm)
    {
        switch (algorithm)
        {
            case EncodeAlgorithms.Eof:
                _image = repository.LoadImageToBytes("encode", imagePath);
                _encodedImage = Algorithms.EOF.EOFWriter.WritePastEOFMarker(_image, message);
                repository.SaveImageFromBytes(_encodedImage);                
                break;
            case EncodeAlgorithms.Metadata:
                _image = repository.LoadImageToBytes("encode", imagePath);
                _encodedImage = Algorithms.Metadata.MetadataWriter.WriteMessageToMetadata(_image, message);
                repository.SaveImageFromBytes(_encodedImage);
                break;
        }
    }
    
    
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}