using System.Drawing.Imaging;
using System.Drawing;
using Steganography.ConsoleUI;
using Steganography.Repository;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography
{
    class Program
    {
        static void Main()
        {
            IRepository repository = new Repository.Repository();
            ImageEncoder encoder = new ImageEncoder(repository);
            ImageDecoder decoder = new ImageDecoder(repository);
            IConsoleUi consoleUi = new ConsoleUi(encoder, decoder);
            consoleUi.Run();
        }
    }

}