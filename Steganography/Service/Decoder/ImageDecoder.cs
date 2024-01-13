using Steganography.Repository;
using Steganography.Service.Algorithms;
using Steganography.Service.Algorithms.AlphaChannel;
using Steganography.Service.Algorithms.DCT;
using Steganography.Service.Algorithms.EOF;
using Steganography.Service.Algorithms.LSB;
using Steganography.Service.Algorithms.Metadata;

namespace Steganography.Service.Decoder
{
    public class ImageDecoder(IRepository repository) : IImageDecoder
    {
        public string DecodeMessage(string imagePath, string algorithm)
        {
            var decodedMessgae = "";
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
                    var image = repository.LoadImageToBytes("decode", imagePath);
                    decodedMessgae = EOFReader.ReadPastEOFMarker(image);
                    if (decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";
                case EncodeAlgorithms.Metadata:
                    var rgbImage = repository.LoadImageToRGB("decode", imagePath);
                    decodedMessgae = MetadataReader.ReadMessageFromImage(rgbImage);
                    if (decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";
                case EncodeAlgorithms.Lsb:
                    if (extension != "png") throw new Exception("Unsupported image format.");
                    rgbImage = repository.LoadImageToRGB("decode", imagePath);
                    decodedMessgae = LsbReader.ReadMessage(rgbImage);
                    if (decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";
                case EncodeAlgorithms.AlphaChannel:
                    if (extension == "png") throw new Exception("Unsupported image format.");
                    rgbImage = repository.LoadImageToRGB("decode", imagePath);
                    decodedMessgae = AlphaChannelReader.ReadMessage(rgbImage);
                    if (decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";
                case EncodeAlgorithms.Dct:
                    image = repository.LoadImageToBytes("decode", imagePath);
                    decodedMessgae = DCTReader.ReadMessageFromImage(image);
                    if(decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";

                default:
                    throw new Exception("This Method is not Implemented.");
            }
            
        }
        public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("EncodedImages");
    }
}