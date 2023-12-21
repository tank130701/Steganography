using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.AlphaChannelImageDecoder;

internal class AlphaChannelImageDecoder : IImageDecoder
{
    public void DecodeText(string imagePath)
    {
        Bitmap bmp = new Bitmap(imagePath);
        StringBuilder decodedText = new StringBuilder();

        // Предположим, что длина сообщения хранится в альфа-канале первого пикселя
        Color firstPixel = bmp.GetPixel(0, 0);
        int messageLength = firstPixel.A;

        for (int i = 1; i <= messageLength; i++)
        {
            int x = i % bmp.Width;
            int y = i / bmp.Width;

            if (y >= bmp.Height)
            {
                throw new InvalidOperationException("Изображение не содержит полного сообщения.");
            }

            Color pixel = bmp.GetPixel(x, y);
            char character = (char)pixel.A; // Извлекаем символ из альфа-канала
            decodedText.Append(character);
        }

        Console.WriteLine("Decoded Text: " + decodedText.ToString());
    }
}