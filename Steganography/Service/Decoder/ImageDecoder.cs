using Steganography.Repository;
using Steganography.Service.Algorithms;
using Steganography.Service.Algorithms.AlphaChannel;
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
            
            switch (algorithm)
            {
                case EncodeAlgorithms.Eof:
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
                    rgbImage = repository.LoadImageToRGB("decode", imagePath);
                    decodedMessgae = LsbReader.ReadMessage(rgbImage);
                    if (decodedMessgae != "") return decodedMessgae;
                    return "Image does not have a message";
                case EncodeAlgorithms.AlphaChannel:
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