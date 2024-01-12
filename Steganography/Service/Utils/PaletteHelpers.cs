using SixLabors.ImageSharp; // Основной пакет ImageSharp
using SixLabors.ImageSharp.PixelFormats; // Для работы с форматами пикселей


namespace Steganography.Service.Utils;

internal static class PaletteHelpers
{
// Функция для получения палитры изображения
    internal static Rgba32[] GetPalette(Image<Rgba32> image)
    {
        HashSet<Rgba32> colors = new HashSet<Rgba32>();
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                colors.Add(image[x, y]);
            }
        }
        return colors.ToArray();
    }

// Функция для изменения цвета в палитре
    internal static Rgba32 ChangeColor(Rgba32 color, byte bit)
    {
        // Изменение младшего значащего бита красного канала
        if (bit == 1)
        {
            color.R |= 1;
        }
        else
        {
            color.R &= 0xFE;
        }
        return color;
    }

// Функция для применения измененной палитры к изображению
    internal static void ApplyPalette(Image<Rgba32> image, Rgba32[] palette)
    {
        Parallel.For(0, image.Height, y =>
        {
            for (int x = 0; x < image.Width; x++)
            {
                Rgba32 pixel = image[x, y];
                // Находим ближайший цвет в палитре
                Rgba32 closestColor = FindClosestColor(pixel, palette);
                // Применяем цвет к пикселю
                image[x, y] = closestColor;
            }
        });
    }


// Вспомогательная функция для нахождения ближайшего цвета в палитре
    static Rgba32 FindClosestColor(Rgba32 target, Rgba32[] palette)
    {
        Rgba32 closestColor = palette[0];
        double closestDistance = double.MaxValue;
        foreach (Rgba32 color in palette)
        {
            double distance = ColorDistance(target, color);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestColor = color;
            }
        }
        return closestColor;
    }

// Вспомогательная функция для вычисления расстояния между цветами
    static double ColorDistance(Rgba32 color1, Rgba32 color2)
    {
        // Вычисление Евклидова расстояния между двумя цветами
        int redDifference = color1.R - color2.R;
        int greenDifference = color1.G - color2.G;
        int blueDifference = color1.B - color2.B;
        return Math.Sqrt(redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference);
    }
}