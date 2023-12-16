using Steganography.Repository;
using Steganography.Service.Algorithms;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private readonly IRepository _repository = repository;
    
    public void EncodeText(string imagePath, string outputImagePath, string text, string algorithm)
    {
        switch (algorithm)
        {
        }
        // var image = repository.LoadImage(imagePath);
    }
    
    public List<string> GetImageFilesInDirectory() => repository.GetImageFilesInDirectory("ImagesForEncoding");

}