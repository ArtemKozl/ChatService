using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;

namespace ChatBack.Infrastructure
{
    public class ImageFileSystem : IImageFileSystem
    {
        public async Task<byte[]> ReadFileToByteArrayAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл не найден: {filePath}");
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> SaveImageAsUniqueFile(byte[] imageData)
        {

            var uniqueFileName = $"{Guid.NewGuid()}.jpg";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", uniqueFileName);

            using var memoryStream = new MemoryStream(imageData);
            using var image = Image.Load(memoryStream);

            await Task.Run(() =>
            {
                image.Save(filePath, new JpegEncoder());
                image.Dispose();
            });

            return filePath;
        }
    }
}
