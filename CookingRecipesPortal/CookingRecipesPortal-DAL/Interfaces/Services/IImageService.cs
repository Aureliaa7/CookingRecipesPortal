namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IImageService
    {
        Task<IList<string>> SaveImagesAsync(IList<string> base64Images, string destinationPath);

        Task DeleteImagesAsync(IList<string> filePaths);

        Task<byte[]> GetImageContentAsync(string filePath);
    }
}
