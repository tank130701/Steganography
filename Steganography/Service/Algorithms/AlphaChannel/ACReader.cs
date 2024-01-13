using System;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.AlphaChannel;

internal static class AlphaChannelReader
{
    internal static string ReadMessage(Image<Rgba32> image)
    {
        StringBuilder message = new StringBuilder();

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];

                // Read the message from the alpha channel
                byte alpha = (byte)(pixel.A & 1);
                message.Append(alpha);
            }
        }

        // Convert binary string to ASCII string
        string binaryMessage = message.ToString();
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < binaryMessage.Length; i += 8)
        {
            string binaryByte = binaryMessage.Substring(i, 8);
            byte asciiByte = Convert.ToByte(binaryByte, 2);
            if (asciiByte == 255)
                break;
            result.Append((char)asciiByte);
        }

        return result.ToString();
    }
}