using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.AlphaChannelImageDecoder;

public class AlphaChannelImageEncoder
{
    public void EncodeText(string imagePath, string outputImagePath, string message)
    {
        Bitmap bmp = new Bitmap(imagePath);
        int messageIndex = 0;

        // Убедитесь, что сообщение помещается в изображение
        if (message.Length > bmp.Width * bmp.Height - 1)
        {
            throw new InvalidOperationException("Сообщение слишком велико для данного изображения.");
        }
        // Сохраняем длину сообщения в альфа-канале первого пикселя
        Color firstPixel = bmp.GetPixel(0, 0);
        Color newFirstPixel = Color.FromArgb(message.Length, firstPixel.R, firstPixel.G, firstPixel.B);
        bmp.SetPixel(0, 0, newFirstPixel);
        
        for (int i = 1; i <= message.Length; i++)
        {
            int x = i % bmp.Width;
            int y = i / bmp.Width;

            Color pixel = bmp.GetPixel(x, y);
            char character = message[messageIndex++];
            Color newPixel = Color.FromArgb(character, pixel.R, pixel.G, pixel.B);
            bmp.SetPixel(x, y, newPixel);
        }
        
        bmp.Save(outputImagePath, ImageFormat.Png);
    }
}