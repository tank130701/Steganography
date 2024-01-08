using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.AlphaChannel;

internal class AlphaChannelWriter
{
    internal static byte[] WriteMessage(Image<Rgba32> image, string message)
    {
        StringBuilder binaryMessage = new StringBuilder();

        // Convert ASCII characters to binary string
        foreach (char character in message)
        {
            string binaryChar = Convert.ToString(character, 2).PadLeft(8, '0');
            binaryMessage.Append(binaryChar);
        }

        int bitCount = 0;

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];

                // Clear the least significant bit of the alpha channel
                pixel.A &= 0xFE;

                // Write the message bit to the least significant bit of the alpha channel
                pixel.A |= (byte)(binaryMessage[bitCount] - '0');

                image[x, y] = pixel;

                bitCount++;

                // Break if the entire message is written
                if (bitCount >= binaryMessage.Length)
                {
                    break;
                }
            }
            if (bitCount >= binaryMessage.Length)
            {
                break;
            }
        }

        using (MemoryStream outputStream = new MemoryStream())
        {
            image.SaveAsPng(outputStream);
            return outputStream.ToArray();
        }
    }
}