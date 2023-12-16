using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Steganography.Repository
{
    public class Repository : IRepository
    {
        public Bitmap LoadImageToBitmap(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Файл не найден", imagePath);
            }

            return new Bitmap(imagePath);
        }

        public string SaveImageFromBitmap(Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Изображение не может быть null");
            }

            string path = Path.Combine("path_to_save_directory", $"{Guid.NewGuid()}.png");

            // Сохранение в формате PNG, можно выбрать другой формат, если нужно
            image.Save(path, ImageFormat.Png);

            return path;
        }

        public byte[] LoadImageToBytes(string imagePath)
        {
              if (!File.Exists(imagePath))
         {
             throw new FileNotFoundException("Файл не найден", imagePath);
         }

         using (Image image = Image.FromFile(imagePath))
         {
             using (MemoryStream ms = new MemoryStream())
             {
                 // Сохранение изображения в MemoryStream
                 image.Save(ms, image.RawFormat);

                 return ms.ToArray();
             }
         }
        }

        public string SaveImageFromBytes(byte[] image)
        {
                if (image == null || image.Length == 0)
                {
                    throw new ArgumentException("Данные изображения пусты", nameof(image));
                }

                string path = Path.Combine("path_to_save_directory", $"{Guid.NewGuid()}.png");

                using (MemoryStream ms = new MemoryStream(image))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        // Сохранение изображения
                        img.Save(path, ImageFormat.Png);
                    }
                }

                return path;
            }
        

        public List<string> GetImageFilesInDirectory(string directoryPath)
        {
            List<string> imageFileList = new List<string>();
            const string root = "./";
            try
            {
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(root + directoryPath, "*" + extension);
                    imageFileList.AddRange(files);
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Error when getting the list of images: {ex.Message}", ex);
            }

            return imageFileList;
        }
    }
}
