using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Text;

namespace Steganography.Service.Algorithms.Metadata;

internal static class MetadataReader
{
    internal static string ReadMessageFromImage(Image<Rgba32> image)
    {
        // Получаем ExifProfile изображения
        ExifProfile profile = image.Metadata.ExifProfile;
        if (profile == null)
        {
            throw new InvalidOperationException("No Exif profile found in image.");
        }

        // Пытаемся получить пользовательский комментарий
        if (!profile.TryGetValue(ExifTag.UserComment, out var value))
        {
            throw new InvalidOperationException("No user comment found in Exif profile.");
        }

        // Предполагаем, что комментарий - это первое значение
        var userComment = value.GetValue();
        if (userComment == null)
        {
            throw new InvalidOperationException("User comment is null.");
        }

        // Конвертируем комментарий в строку
        return userComment.ToString();
    }
}