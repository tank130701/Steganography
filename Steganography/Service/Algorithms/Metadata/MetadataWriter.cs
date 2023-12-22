using System.Text;

namespace Steganography.Service.Algorithms.Metadata;

internal static class MetadataWriter
{
    internal static byte[] WriteMessageToMetadata(byte[] byteArray, string message)
    {
        if (byteArray == null)
        {
            throw new ArgumentNullException(nameof(byteArray));
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Сообщение не может быть пустым.", nameof(message));
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

        // Создаем новый массив байт для записи сообщения в метаданные
        byte[] newData = new byte[byteArray.Length + message.Length];

        // Копируем данные до сегмента приложения
        Array.Copy(byteArray, 0, newData, 0, index + 4);

        // Копируем данные после сегмента приложения
        Array.Copy(byteArray, index + 4, newData, index + 4 + message.Length, byteArray.Length - index - 4);

        // Записываем длину сообщения в два байта в сегмент приложения
        newData[index + 2] = (byte)((message.Length + 2) >> 8);
        newData[index + 3] = (byte)((message.Length + 2) & 0xFF);

        // Записываем сообщение в сегмент приложения
        Encoding.UTF8.GetBytes(message).CopyTo(newData, index + 4);

        return newData;
    }
}