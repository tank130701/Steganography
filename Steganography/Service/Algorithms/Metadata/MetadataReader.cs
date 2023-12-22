using System.Text;
namespace Steganography.Service.Algorithms.Metadata;

internal static class MetadataReader
{
    internal static string ReadMessageFromMetadata(byte[] byteArray)
    {
        if (byteArray == null)
        {
            throw new ArgumentNullException(nameof(byteArray));
        }

        // Идентификаторы сегментов JPEG
        const byte StartOfImageMarker = 0xFF;
        const byte ApplicationSpecificMarker = 0xE1;

        // Ищем сегмент приложения в изображении JPEG
        int index = 2; // Пропускаем маркер начала изображения (SOI)
        while (index < byteArray.Length - 1)
        {
            if (byteArray[index] == StartOfImageMarker && byteArray[index + 1] == ApplicationSpecificMarker)
            {
                // Найден сегмент приложения
                break;
            }
            index++;
        }

        if (index >= byteArray.Length - 1)
        {
            throw new InvalidOperationException("Изображение JPEG не содержит сегмента приложения.");
        }

        // Размер данных в сегменте приложения
        int dataSize = (byteArray[index + 2] << 8) + byteArray[index + 3] - 2;

        // Извлекаем сообщение из сегмента приложения
        string message = Encoding.UTF8.GetString(byteArray, index + 4, dataSize);

        return message;
    }
}