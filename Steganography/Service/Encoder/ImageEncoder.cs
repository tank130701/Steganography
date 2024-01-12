using Steganography.Repository;
using Steganography.Service.Algorithms;
using Steganography.Service.Algorithms.AlphaChannel;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private byte[] _image;
    private byte[] _encodedImage;
    public string EncodeMessage(string imagePath, string message, string algorithm)
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

        string extension = Path.GetExtension(imagePath).ToLowerInvariant();

        switch (extension)
        {
            case ".png":
                extension = "png";
                break;
            case ".jpeg":
                extension = "jpeg";
                break;
            case ".jpg":
                extension = "jpg";
                break;
            default:
                throw new Exception("Unsupported image format.");
        }

        switch (algorithm)
        {
            case EncodeAlgorithms.Eof:
                if (extension == "png") throw new Exception("Unsupported image format.");
                _image = repository.LoadImageToBytes("encode", imagePath);
                _encodedImage = Algorithms.EOF.EOFWriter.WritePastEOFMarker(_image, message);
                return repository.SaveImageFromBytes(_encodedImage, extension);                
            case EncodeAlgorithms.Metadata:
                var rgbImage = repository.LoadImageToRGB("encode", imagePath);
                var RGBEncodedImage = Algorithms.Metadata.MetadataWriter.HideMessageInImage(rgbImage, message);
                return repository.SaveImageFromRGB(RGBEncodedImage, extension);
            case EncodeAlgorithms.Lsb:
                if (extension != "png") throw new Exception("Unsupported image format.");
                rgbImage = repository.LoadImageToRGB("encode", imagePath);
                var rgbEncodedImage = Algorithms.LSB.LsbWriter.WriteMessage(rgbImage, message);
                return repository.SaveImageFromRGB(rgbEncodedImage, extension);  
            case EncodeAlgorithms.AlphaChannel:
                if (extension == "png") throw new Exception("Unsupported image format.");
                rgbImage = repository.LoadImageToRGB("encode", imagePath);
                _encodedImage = AlphaChannelWriter.WriteMessage(rgbImage, message);
                return repository.SaveImageFromBytes(_encodedImage, extension);
            case EncodeAlgorithms.Dct:
                if(!(extension=="jpeg" || extension == "jpg")) throw new Exception($"Unsupported image extension:{extension}");
                _image = repository.LoadImageToBytes("encode",imagePath);
                _encodedImage = DCTWriter.WriteStegoMessage(_image, message);
                // placeholder
                return repository.SaveImageFromBytes(_encodedImage, extension);
            default:
                throw new Exception("This Method is not Implemented.");
        }
    }
    
    
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}