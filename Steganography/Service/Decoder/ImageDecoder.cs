using Steganography.Repository;

namespace Steganography.Service.Decoder
{
    public class ImageDecoder(IRepository repository) : IImageDecoder
    {
        private readonly IRepository _repository = repository;
        public void DecodeText(string imagePath)
        {
            throw new NotImplementedException();
        }
    }
}