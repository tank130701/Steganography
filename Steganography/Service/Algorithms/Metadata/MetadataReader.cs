using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
namespace Steganography.Service.Algorithms.Metadata;

public static class MetadataReader
{
    public static string ReadMessageFromImage(Image<Rgba32> image)
    {
        {
            // Чтение сообщения из метаданных изображения
            byte[] messageBytes = new byte[image.Width * image.Height * 3 / 8];
            int messageIndex = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];

                    // Чтение младших битов компонент цвета пикселя в байты сообщения
                    if (messageIndex < messageBytes.Length)
                    {
                        byte messageByte = 0;
                        messageByte |= (byte)((pixel.R & 0x01) << 7);
                        messageByte |= (byte)((pixel.G & 0x01) << 6);
                        messageByte |= (byte)((pixel.B & 0x01) << 5);
                        messageBytes[messageIndex] = messageByte;
                        messageIndex++;
                    }
                }
            }

            // Преобразование байтов сообщения в строку
            string message = System.Text.Encoding.UTF8.GetString(messageBytes);
            return message;
        }
    }
}