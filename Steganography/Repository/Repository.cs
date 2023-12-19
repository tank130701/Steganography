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
        private const string TextRoot = "./";
        private const string TextEncodedImagesDirectory = "./EncodedImages/";
        private const string TextImagesForEncodingDirectory = "./ImagesForEncoding/";

        public Repository()
        {
            if (!Directory.Exists(TextEncodedImagesDirectory))
            {
                Directory.CreateDirectory(TextEncodedImagesDirectory);
                Console.WriteLine("Directory EncodedImages has been created");
            }

            if (!Directory.Exists(TextImagesForEncodingDirectory))
            {
                Directory.CreateDirectory(TextImagesForEncodingDirectory);
                Console.WriteLine("Directory ImagesForEncoding has been created");
            }
        }

        public Bitmap LoadImageToBitmap(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("FileNotFound", imagePath);
            }

            return new Bitmap(imagePath);
        }

        public string SaveImageFromBitmap(Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image cannot be null!");
            }
            
            Guid guid = Guid.NewGuid();
            var uuid = guid.ToString();
            var newImage = $"{uuid}.jpg";
            var newImagePath = TextEncodedImagesDirectory + newImage;
            
            image.Save(newImagePath, ImageFormat.Png);

            return newImagePath;
        }

        public byte[] LoadImageToBytes(string imageFolder, string imageName)
        {
            var imagePath = "";
            switch (imageFolder)
            {
                case "encode": imagePath = TextImagesForEncodingDirectory + imageName; break;
                case "decode": imagePath = TextEncodedImagesDirectory + imageName; break;
            }
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
                    throw new ArgumentException("Image cannot be null!", nameof(image));
                }
                
                Guid guid = Guid.NewGuid();
                var uuid = guid.ToString();
                var newImage = $"{uuid}.jpg";
                var newImagePath = TextEncodedImagesDirectory + newImage;
                
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
                    string[] files = Directory.GetFiles(TextRoot + directoryPath, "*" + extension);
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        imageFileList.Add(fileName);
                    }
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
