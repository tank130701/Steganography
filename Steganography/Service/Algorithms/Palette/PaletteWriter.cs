using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Steganography.Service.Utils;

namespace Steganography.Service.Algorithms.Palette;

internal static class PaletteWriter
{
    internal static byte[] WriteMessage(Image<Rgba32> image, string message)
    {
        // Преобразование сообщения в двоичный формат
        StringBuilder binaryMessage = new StringBuilder();
        foreach (char character in message)
        {
            string binaryChar = Convert.ToString(character, 2).PadLeft(8, '0');
            binaryMessage.Append(binaryChar);
        }

        // Получение палитры изображения
        var palette = PaletteHelpers.GetPalette(image);
        int bitCount = 0;

        // Изменение палитры для встраивания сообщения
        for (int i = 0; i < palette.Length; i++)
        {
            // Модификация цвета в палитре
            Rgba32 color = palette[i];
            byte lastBit = (byte)(binaryMessage[bitCount] - '0');
            color = PaletteHelpers.ChangeColor(color, lastBit);

            // Обновление палитры
            palette[i] = color;
            bitCount++;

            // Проверка на окончание сообщения
            if (bitCount >= binaryMessage.Length)
            {
                break;
            }
        }

        // Применение измененной палитры к изображению
        PaletteHelpers.ApplyPalette(image, palette);

        // Сохранение изображения
        using (MemoryStream outputStream = new MemoryStream())
        {
            image.SaveAsPng(outputStream);
            return outputStream.ToArray();
        }
    }
}