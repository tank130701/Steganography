using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Steganography.Service.Encoder
{
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

    public class MetadataImageEncoder
    {
        public void EncodeText(string imagePath, string outputImagePath, string message)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                // Идентификатор комментария EXIF
                const int ExifCommentId = 0x9286;

                // Создаем новый элемент свойства для хранения текста
                PropertyItem propItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                propItem.Id = ExifCommentId;
                propItem.Type = 2; // ASCII
                propItem.Value = Encoding.ASCII.GetBytes(message);
                propItem.Len = propItem.Value.Length;

                // Добавляем или заменяем свойство в изображении
                bool found = false;
                for (int i = 0; i < image.PropertyItems.Length; i++)
                {
                    if (image.PropertyItems[i].Id == ExifCommentId)
                    {
                        image.PropertyItems[i] = propItem;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    image.SetPropertyItem(propItem);
                }

                // Сохраняем изображение с новыми метаданными
                image.Save(outputImagePath, ImageFormat.Jpeg); // или другой формат, если требуется
            }
        }
    }

} 



