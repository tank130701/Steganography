using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.LSB;

 public static class LsbReader
    {
        public static string ReadMessage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            using (Image<Rgba32> image = Image.Load<Rgba32>(stream))
            {
                int messageLength = 0;

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];

                        // Read the least significant bit of each color channel
                        byte lsb = (byte)(pixel.R & 1 | (pixel.G & 1) << 1 | (pixel.B & 1) << 2);

                        // Add the bit to the message
                        messageLength = messageLength << 1 | lsb;

                        // Break if the message length is reached
                        if (messageLength >= 8)
                        {
                            break;
                        }
                    }
                    if (messageLength >= 8)
                    {
                        break;
                    }
                }

                byte[] messageBytes = new byte[messageLength];
                int bitCount = 0;

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];

                        // Read the least significant bit of each color channel
                        byte lsb = (byte)(pixel.R & 1 | (pixel.G & 1) << 1 | (pixel.B & 1) << 2);

                        // Add the bit to the message
                        messageBytes[bitCount >> 3] = (byte)(messageBytes[bitCount >> 3] << 1 | lsb);
                        bitCount++;

                        // Break if the entire message is read
                        if (bitCount >= messageLength * 8)
                        {
                            break;
                        }
                    }
                    if (bitCount >= messageLength * 8)
                    {
                        break;
                    }
                }
                return Encoding.UTF8.GetString(messageBytes);
            }
        }
        
    }
    