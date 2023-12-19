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
            }
        }
}