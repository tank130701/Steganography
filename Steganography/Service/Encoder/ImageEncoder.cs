using Steganography.Repository;
using Steganography.Service.Algorithms;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private byte[] _image;
    private byte[] _encodedImage;
    public void EncodeMessage(string imagePath, string message, string algorithm)
    {
        if (message is "The message is empty. Write a message." or "")
        {
            throw new Exception("The message is empty. Write a message.");
        }

        if (algorithm == "The encoding algorithm is not selected. Select an algorithm.")
        {
            throw new Exception("The encoding algorithm is not selected. Select an algorithm.");
        }

        if (imagePath == "The file is not selected. Select a file.")
        {
            throw new Exception("The file is not selected. Select a file.");
        }
        
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
            default:
                throw new Exception("This Method is not Implemented.");
        }
    }
    
    
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}