using System.Drawing;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Text.Encoding;

namespace Steganography.Service.MetadataImage;

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