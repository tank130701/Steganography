using Steganography.Repository;
using Steganography.Service.Algorithms;

namespace Steganography.Service.Decoder
{
    public class ImageDecoder(IRepository repository) : IImageDecoder
    {
        public string DecodeMessage(string imagePath, string algorithm)
        {
            switch (algorithm)
            {
                case EncodeAlgorithms.Eof:
                    var image = repository.LoadImageToBytes(imagePath);
                    var decodedMessgae = EOFReader.ReadPastEOFMarker(image);
                    if (decodedMessgae != null) return decodedMessgae;
                    return "Image does not have a message";
            }

            return "";
        }
        public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("EncodedImages");
    }
}