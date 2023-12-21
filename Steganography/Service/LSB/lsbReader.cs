using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.LSB;

public class ImageEncoder
{
    public void EncodeText(string imagePath, string outputImagePath, string message, string algorithm)
    {
        Bitmap bmp = new Bitmap(imagePath);
        int messageIndex = 0;

        for (int i = 0; i < bmp.Height; i++)
        {
            for (int j = 0; j < bmp.Width; j++)
            {
                if (messageIndex == message.Length) break;

                Color pixel = bmp.GetPixel(j, i);
                char character = message[messageIndex++];

                int value = character;
                int R = (value >> 16) & 255;
                int G = (value >> 8) & 255;
                int B = value & 255;

                Color newPixel = Color.FromArgb(R, G, B);
                bmp.SetPixel(j, i, newPixel);
            }

            if (messageIndex == message.Length) break;
        }

        // Сохраняем длину сообщения в последнем пикселе
        Color lastPixel = bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
        Color updatedLastPixel = Color.FromArgb(lastPixel.R, lastPixel.G, message.Length);
        bmp.SetPixel(bmp.Width - 1, bmp.Height - 1, updatedLastPixel);

        bmp.Save(outputImagePath, ImageFormat.Png);
    }

    public List<string> GetImageFilesInDirectory()
    {
        // Реализация загрузки списка файлов изображений из директории
        throw new NotImplementedException();
    }
}