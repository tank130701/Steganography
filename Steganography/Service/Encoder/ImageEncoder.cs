using Steganography.Repository;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private readonly IRepository _repository = repository;
    
    public void EncodeText(string imagePath, string outputImagePath, string text, string algorithm)
    {
        throw new NotImplementedException();
    }
}