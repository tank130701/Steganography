using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.LSB;

internal static class LsbReader
{
    internal static string ReadMessage(Image<Rgba32> image)
    {
        int messageLength = 0;
        for (int i = 0; i < 32; i++)
        {
            Rgba32 pixel = image[0, i];
            messageLength |= ((pixel.R & 1) << (7 - (i % 8)));
        }

        byte[] messageBytes = new byte[messageLength];
        int bitCount = 0;

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = (y == 0 ? 32 : 0); x < image.Width; x++)
            {
                if (bitCount >= messageLength * 8)
                {
                    break;
                }

                Rgba32 pixel = image[x, y];

                messageBytes[bitCount / 8] = (byte)((messageBytes[bitCount / 8] << 1) | (pixel.R & 1));
                bitCount++;
            }
        }
        return Encoding.UTF8.GetString(messageBytes);
    }
}
    