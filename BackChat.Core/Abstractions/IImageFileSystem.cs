
namespace ChatBack.Infrastructure
{
    public interface IImageFileSystem
    {
        Task<byte[]> ReadFileToByteArrayAsync(string filePath);
        Task<string> SaveImageAsUniqueFile(byte[] imageData);
    }
}