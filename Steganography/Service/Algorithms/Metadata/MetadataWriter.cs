using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Steganography.Service.Algorithms.Metadata;

internal static class MetadataWriter
{
    public static Image<Rgba32> HideMessageInImage(Image<Rgba32> image, string message)
    {
        // Преобразование сообщения в байтовый массив
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
    
        // Проверка, достаточно ли места в метаданных изображения для записи сообщения
        int maxMessageLength = image.Width * image.Height * 3 / 8;
        if (messageBytes.Length > maxMessageLength)
        {
            throw new Exception("Сообщение слишком длинное для данного изображения");
        }

        // Запись сообщения в метаданные изображения
        int messageIndex = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];

                // Запись битов сообщения в младшие биты компонент цвета пикселя
                if (messageIndex < messageBytes.Length)
                {
                    byte messageByte = messageBytes[messageIndex];
                    pixel.R = (byte)((pixel.R & 0xFE) | ((messageByte >> 7) & 0x01));
                    pixel.G = (byte)((pixel.G & 0xFE) | ((messageByte >> 6) & 0x01));
                    pixel.B = (byte)((pixel.B & 0xFE) | ((messageByte >> 5) & 0x01));
                    messageIndex++;
                }

                image[x, y] = pixel;
            }
        }
        return image;
    }
}