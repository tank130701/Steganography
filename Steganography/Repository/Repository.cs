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
        private const string _root = "./";
        private const string _encodedImagesDirectory = "./EncodedImages/";
        private const string _imagesForEncodingDirectory = "./ImagesForEncoding/";
        
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
            Guid guid = Guid.NewGuid();
            var uuid = guid.ToString();
            var newImage = $"{uuid}.jpg";
            var newImagePath = _encodedImagesDirectory + newImage;
            
            image.Save(newImagePath, ImageFormat.Png);

            return newImagePath;
        }

        public byte[] LoadImageToBytes(string imagePath)
        { 
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("FileNotFound", imagePath);
            }

            using FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using BinaryReader br = new BinaryReader(fs);
            var fileBytes = br.ReadBytes((int)fs.Length);

            return fileBytes;
        }

        public string SaveImageFromBytes(byte[] image)
        {
                if (image == null || image.Length == 0)
                {
                    throw new ArgumentException("Данные изображения пусты", nameof(image));
                }
                
                Guid guid = Guid.NewGuid();
                var uuid = guid.ToString();
                var newImage = $"{uuid}.jpg";
                var newImagePath = _encodedImagesDirectory + newImage;
                
                using (FileStream fs = new FileStream(newImagePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(image);
                    }
                }

                return newImagePath;
        }
        

        public List<string> GetImageFilesInDirectory(string directoryPath)
        {
            List<string> imageFileList = new List<string>();
            try
            {
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(_root + directoryPath, "*" + extension);
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
