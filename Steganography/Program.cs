using System.Drawing.Imaging;
using System.Drawing;

public static class SteganographyHelper
{
    public static void EncodeText(string imagePath, string outputImagePath, string text)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            throw new ArgumentException("Image path cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to be hidden cannot be null or empty.");
        }

        Bitmap image = new Bitmap(imagePath);
        var textSize = text.Length * 8; // Assuming each character is represented by 16 bits
        var textSizeInKB = textSize / 1024;

        if (textSizeInKB > GetImageSizeInKB(image))
        {
            throw new Exception("Image cannot save text more than " + GetImageSizeInKB(image) + " KB");
        }

        HideTextInImage(image, text);

        image.Save(outputImagePath, ImageFormat.Png);
    }

    public static string DecodeText(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            throw new ArgumentException("Image path cannot be null or empty.");
        }

        Bitmap image = new Bitmap(imagePath);

        return ExtractTextFromImage(image);
    }

    private static void HideTextInImage(Bitmap image, string text)
    {
        int textLength = text.Length;

        int charIndex = 0;
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                Color pixel = image.GetPixel(i, j);

                if (charIndex < textLength)
                {
                    char letter = text[charIndex];
                    int value = Convert.ToInt32(letter);
                    Color modifiedColor = ModifyPixelColor(value);
                    image.SetPixel(i, j, modifiedColor);
                }

                if (i == image.Width - 1 && j == image.Height - 1)
                {
                    image.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, textLength));
                }

                charIndex++;
            }
        }
    }

    private static Color ModifyPixelColor(int value)
    {
        // Extract the individual bytes from the integer value
        byte byte3 = (byte)((value & 0xFF000000) >> 24);
        byte byte2 = (byte)((value & 0x00FF0000) >> 16);
        byte byte1 = (byte)((value & 0x0000FF00) >> 8);
        byte byte0 = (byte)(value & 0x000000FF);

        // Create a new Color object using the extracted bytes
        return Color.FromArgb(byte3, byte2, byte1, byte0);
    }

    private static string ExtractTextFromImage(Bitmap image)
    {
        string extractedText = "";
        Color lastPixel = image.GetPixel(image.Width - 1, image.Height - 1);
        int textLength = lastPixel.B;

        int charIndex = 0;
        for (int i = 0; i < image.Width; i++)
        {
            for (int j = 0; j < image.Height; j++)
            {
                if (charIndex < textLength)
                {
                    Color pixel = image.GetPixel(i, j);
                    int value = pixel.B;
                    char c = Convert.ToChar(value);
                    extractedText += c;
                }

                charIndex++;
            }
        }

        return extractedText;
    }

    private static double GetImageSizeInKB(Bitmap image)
    {
        return (image.Width * image.Height * 16) / 1024.0; // Assuming each pixel stores 16 bits
    }
}

class Program
{
    static void Main()
    {
        string imagePath = "original_image.png";
        string outputImagePath = "output.png";
        string textToHide = "Hello World!";

        SteganographyHelper.EncodeText(imagePath, outputImagePath, textToHide);
        Console.WriteLine("Text encoded successfully.");

        string extractedText = SteganographyHelper.DecodeText(outputImagePath);
        Console.WriteLine("Extracted Text: " + extractedText);
    }
}