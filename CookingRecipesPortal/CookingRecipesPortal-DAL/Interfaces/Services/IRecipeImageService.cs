namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IRecipeImageService
    {
        Task SaveRecipeImagesAsync(IList<string> base64Images, Guid recipeId);

        Task<IList<byte[]>> GetRecipeImagesAsync(Guid recipeId);

        Task DeleteRecipeImagesAsync(Guid recipeId);
    }
}
