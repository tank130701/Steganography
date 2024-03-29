﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Jpeg;

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
        
        public Image<Rgba32> LoadImageToRGB(string imageFolder, string imageName)
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
            return Image.Load<Rgba32>(imagePath);
        }

        public string SaveImageFromRGB(Image<Rgba32> image, string imageType)
        {
            Guid guid = Guid.NewGuid();
            var uuid = guid.ToString();
            var newImage = $"{uuid}.{imageType}";
            var newImagePath = TextEncodedImagesDirectory + newImage;
            image.Save(newImagePath);
            return newImage;
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

        public string SaveImageFromBytes(byte[] image, string imageType)
        {
                if (image == null || image.Length == 0)
                {
                    throw new ArgumentException("Image cannot be null!", nameof(image));
                }
                
                Guid guid = Guid.NewGuid();
                var uuid = guid.ToString();
                var newImage = $"{uuid}.{imageType}";
                var newImagePath = TextEncodedImagesDirectory + newImage;
                
                using (FileStream fs = new FileStream(newImagePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(image);
                    }
                }

                return newImage;
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
