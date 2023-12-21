using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.LSB;

internal class ImageDecoder
{
    public void DecodeText(string imagePath)
    {
        Bitmap bmp = new Bitmap(imagePath);

        StringBuilder decodedText = new StringBuilder();

        Color lastPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
        int messageLength =
            lastPixel.B; // Предположим, что длина сообщения хранится в синем канале последнего пикселя

        for (int i = 0, colorIndex = 0; i < messageLength; i++)
        {
            Color pixel = bmp.GetPixel(colorIndex % bmp.Width, colorIndex / bmp.Width);

            // Извлекаем биты символа из каждого канала цвета
            int value = pixel.R;
            value = (value << 8) + pixel.G;
            value = (value << 8) + pixel.B;

            char c = (char)value; // Преобразуем значение в символ

            decodedText.Append(c); // Добавляем символ к декодированному тексту

            colorIndex++;
        }

        Console.WriteLine("Decoded Text: " + decodedText.ToString());
    }
}