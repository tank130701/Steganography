using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Steganography.Service.Utils;

namespace Steganography.Service.Algorithms.Palette;

internal static class PaletteReader
{
    internal static string ReadMessage(Image<Rgba32> image)
    {
        // Получение палитры изображения
        Rgba32[] palette = PaletteHelpers.GetPalette(image);

        StringBuilder binaryMessage = new StringBuilder();

        // Чтение сообщения из измененных цветов палитры
        foreach (Rgba32 color in palette)
        {
            // Чтение младшего значащего бита красного канала
            byte bit = (byte)(color.R & 1);
            binaryMessage.Append(bit);
        }

        // Преобразование двоичной строки в ASCII строку
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < binaryMessage.Length; i += 8)
        {
            if (i + 8 > binaryMessage.Length) break; // Проверка на выход за пределы строки
            string binaryByte = binaryMessage.ToString().Substring(i, 8);
            byte asciiByte = Convert.ToByte(binaryByte, 2);
            if (asciiByte == 255) break; // Условие окончания сообщения
            result.Append((char)asciiByte);
        }

        return result.ToString();
    }

}