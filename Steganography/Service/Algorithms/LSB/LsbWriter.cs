using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.LSB;

public static class LsbWriter
{
    public static byte[] WriteMessage(byte[] byteArray, string message)
    {
        using (MemoryStream stream = new MemoryStream(byteArray))
        using (Image<Rgba32> image = Image.Load<Rgba32>(stream))
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);

            int bitCount = 0;
            int messageLength = messageBytes.Length * 8;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];

                    // Clear the least significant bit of each color channel
                    pixel.R &= 0xFE;
                    pixel.G &= 0xFE;
                    pixel.B &= 0xFE;

                    // Write the message bit to the least significant bit of each color channel
                    pixel.R |= (byte)((messageBytes[bitCount >> 3] >> (7 - (bitCount % 8))) & 1);
                    pixel.G |= (byte)((messageBytes[bitCount >> 3] >> (7 - (bitCount % 8)) & 1) << 1);
                    pixel.B |= (byte)((messageBytes[bitCount >> 3] >> (7 - (bitCount % 8)) & 1) << 2);

                    image[x, y] = pixel;

                    bitCount++;

                    // Break if the entire message is written
                    if (bitCount >= messageLength)
                    {
                        break;
                    }
                }
                if (bitCount >= messageLength)
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
}
    