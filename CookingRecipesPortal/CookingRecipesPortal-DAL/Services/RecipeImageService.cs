using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;

namespace CookingRecipesPortal_DAL.Services
{
    public class RecipeImageService : IRecipeImageService
    {
        private readonly IImageService imageService;
        private readonly IUnitOfWork unitOfWork;

        public RecipeImageService(IImageService imageService, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
        }

        public async Task<IList<byte[]>> GetRecipeImagesAsync(Guid recipeId)
        {
            bool recipeExists = await unitOfWork.RecipesRepository.ExistsAsync(x => x.Id == recipeId);
            if (!recipeExists)
            {
                throw new EntityNotFoundException($"The recipe with id {recipeId} was not found!");
            }

            var imagesPaths = await GetImagesPathsAsync(recipeId);
            var recipeImagesContent = new List<byte[]>();

            foreach (var path in imagesPaths)
            {
                var imageContent = await imageService.GetImageContentAsync(path);
                recipeImagesContent.Add(imageContent);
            }

            return recipeImagesContent;
        }

        private async Task<List<string>> GetImagesNamesAsync(Guid recipeId)
        {
            var imagesNames = (await unitOfWork.RecipeImagesRepository.GetAllAsync(x => x.RecipeId == recipeId))
                .Select(x => x.ImageName).ToList();

            return imagesNames;
        }

        public async Task SaveRecipeImagesAsync(IList<string> base64Images, Guid recipeId)
        {
            var imagesNames = await imageService.SaveImagesAsync(base64Images, Constants.ImagesPath);
            foreach (var image in imagesNames)
            {
                await unitOfWork.RecipeImagesRepository.AddAsync(
                    new RecipeImage
                    {
                        RecipeId = recipeId,
                        ImageName = image
                    });
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRecipeImagesAsync(Guid recipeId)
        {
            var imagesPaths = await GetImagesPathsAsync(recipeId);
            await imageService.DeleteImagesAsync(imagesPaths);

            var imageRecipesIdsToBeDeleted = (await unitOfWork.RecipeImagesRepository
                .GetAllAsync(x => x.RecipeId == recipeId))
                .Select(x => x.Id)
                .ToList();

            foreach (var imageRecipeId in imageRecipesIdsToBeDeleted)
            {
                await unitOfWork.RecipeImagesRepository.RemoveAsync(imageRecipeId);
            }

            await unitOfWork.SaveChangesAsync();
        }

        private async Task<List<string>> GetImagesPathsAsync(Guid recipeId)
        {
            var imagesNames = await GetImagesNamesAsync(recipeId);
            var imagesPaths = new List<string>();

            foreach (var name in imagesNames)
            {
                imagesPaths.Add($"{Constants.ImagesPath}\\{name}");
            }

            return imagesPaths;
        }
    }
}
