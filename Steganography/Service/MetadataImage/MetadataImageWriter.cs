using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.MetadataImage;

internal class MetadataImageDecoder : IImageDecoder
{
    public void DecodeText(string imagePath)
    {
        using (Image image = Image.FromFile(imagePath))
        {
            // Идентификатор комментария EXIF
            const int ExifCommentId = 0x9286;

            // Находим свойство среди метаданных изображения
            var propItem = image.PropertyItems.FirstOrDefault(item => item.Id == ExifCommentId);

            if (propItem != null)
            {
                // Декодируем значение свойства (предполагая, что оно в формате ASCII)
                string decodedText = Encoding.ASCII.GetString(propItem.Value);

                Console.WriteLine("Decoded Text: " + decodedText);
            }
            else
            {
                Console.WriteLine("No comment property found in image metadata.");
            }
        }
    }
}