using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Steganography.Service.Encoder
{
    public interface IImageEncoder
    {
        void EncodeText(string imagePath, string outputImagePath, string message, string algorithm);
        List<string> GetImageFilesInDirectory();
    }
    public class ImageEncoder : IImageEncoder
        {
            public void EncodeText(string imagePath, string outputImagePath, string message, string algorithm)
                    {
                        Bitmap bmp = new Bitmap(imagePath);
                        int messageIndex = 0;

        }
}