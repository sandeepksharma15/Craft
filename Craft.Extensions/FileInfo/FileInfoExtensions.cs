namespace Microsoft.Extensions.FileProviders;

public static class FileInfoExtensions
{
    public static string ContentType(this IFileInfo file)
        => file.GetImageExtension() switch
        {
            ImageExtension.Jpg => "image/jpeg",
            ImageExtension.Gif => "image/gif",
            ImageExtension.Png => "image/png",
            ImageExtension.Svg => "image/svg",
            _ => "application/octet-stream",
        };

    public static string Extension(this IFileInfo file)
    {
        int index = file.Name.LastIndexOf('.');

        return index < 0 ? string.Empty : file.Name[index..].ToLower();
    }

    public static ImageExtension GetImageExtension(this IFileInfo file)
        => file.Extension() switch
        {
            ".jpg" => ImageExtension.Jpg,
            ".gif" => ImageExtension.Gif,
            ".png" => ImageExtension.Png,
            ".svg" => ImageExtension.Svg,
            _ => ImageExtension.Unknown
        };
}

public enum ImageExtension
{
    Unknown,
    Jpg,
    Gif,
    Png,
    Svg
}
