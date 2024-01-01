using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.LSB;

public static class LsbWriter
{
    public static Image<Rgba32> WriteMessage(Image<Rgba32> image, string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message); 
        byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);
        int bitCount = 0;

        for (int i = 0; i < 32; i++)
        {
            Rgba32 pixel = image[0, i];
            pixel.R = (byte)((pixel.R & ~1) | ((lengthBytes[i / 8] >> (7 - (i % 8))) & 1));
            image[0, i] = pixel;
        }

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = (y == 0 ? 32 : 0); x < image.Width; x++)
            {
                if (bitCount >= messageBytes.Length * 8)
                {
                    break;
                }

                Rgba32 pixel = image[x, y];

                pixel.R = (byte)((pixel.R & ~1) | ((messageBytes[bitCount / 8] >> (7 - (bitCount % 8))) & 1));
                image[x, y] = pixel;

                bitCount++;
            }
        }
        return image;
    }
}
    