using CookingRecipesPortal_DAL.Interfaces.Services;

namespace CookingRecipesPortal_DAL.Services
{
    public class ImageService : IImageService
    {
        public async Task DeleteImagesAsync(IList<string> filePaths)
        {
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    if (File.Exists(filePath))
                    {
                        await WaitUntilFileIsReadyAsync(filePath)
                            .ContinueWith(t => File.Delete(filePath));
                    }
                }
            }
        }

        public async Task<IList<string>> SaveImagesAsync(IList<string> base64Images)
        {
            var imagesNames = new List<string>();

            foreach (var image in base64Images)
            {
                imagesNames.Add(await SaveImageInFolderAsync(image));
            }

            return imagesNames;
        }

        private async Task<string> SaveImageInFolderAsync(string imageBase64)
        {
            string imageFolder = Constants.ImagesPath;
            // let it be a simple GUID since the base64 string doesn't contain the file name and the file name is not important
            //TODO get the image extension from FE instead of using the one from Constants
            string fileName = Guid.NewGuid().ToString() + Constants.ImageExtension;
            string filePath = Path.Combine(imageFolder, fileName);
            byte[] imageBytes = Convert.FromBase64String(imageBase64);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return fileName;
        }

        private static async Task WaitUntilFileIsReadyAsync(string fileName)
        {
            await Task.Run(() =>
            {
                string path = Path.Combine(string.Concat(Constants.ImagesPath, "\\"), fileName);
                if (!File.Exists(path))
                {
                    throw new IOException("File does not exist!");
                }
                bool isReady = false;
                while (!isReady)
                {
                    try
                    {
                        using FileStream inputStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                        isReady = inputStream.Length > 0;
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(IOException))
                        {
                            isReady = false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            });
        }

        public Task<byte[]> GetImageContentAsync(string filePath)
        {
            return File.ReadAllBytesAsync(filePath);
        }
    }
}
