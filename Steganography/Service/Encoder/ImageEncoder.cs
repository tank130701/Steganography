using Steganography.Repository;
using Steganography.Service.Algorithms;
using Steganography.Service.Algorithms.AlphaChannel;

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
                var rgbImage = repository.LoadImageToRGB("encode", imagePath);
                var RGBEncodedImage = Algorithms.Metadata.MetadataWriter.HideMessageInImage(rgbImage, message);
                repository.SaveImageFromRGB(RGBEncodedImage, "png");
                break;
            case EncodeAlgorithms.Lsb:
                rgbImage = repository.LoadImageToRGB("encode", imagePath);
                var rgbEncodedImage = Algorithms.LSB.LsbWriter.WriteMessage(rgbImage, message);
                repository.SaveImageFromRGB(rgbEncodedImage, "png");  
                break;
            case EncodeAlgorithms.AlphaChannel:
                _image = repository.LoadImageToBytes("encode", imagePath);
                _encodedImage = AlphaChannelWriter.WriteMessage(_image, message);
                repository.SaveImageFromBytes(_encodedImage);  
                break;
            default:
                throw new Exception("This Method is not Implemented.");
        }
    }
    
    
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}