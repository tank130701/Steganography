using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
namespace Steganography.Service.Algorithms.Metadata;

internal static class MetadataWriter
{
    public static Image<Rgba32> HideMessageInImage(Image<Rgba32> image, string message)
    {
        // Конвертируем сообщение в байты
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Создаем пользовательский тег в Exif профиле
        ExifProfile profile = image.Metadata.ExifProfile ?? new ExifProfile();
        profile.SetValue(ExifTag.Software, "Steganography");
        profile.SetValue(ExifTag.UserComment, Encoding.UTF8.GetString(messageBytes));

        // Сохраняем изображение с новым Exif профилем
        image.Metadata.ExifProfile = profile;
        return image;
    }
}