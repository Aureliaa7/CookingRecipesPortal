using CookingRecipesPortal_DAL.DomainModels;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface ICookingRecipeService
    {
        Task<Recipe> AddAsync(Guid userId, Recipe recipe);

        Task<IList<Recipe>> GetByUserAsync(Guid userId);
    }
}
