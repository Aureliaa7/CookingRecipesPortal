using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface ICookingRecipeService
    {
        Task<Recipe> AddAsync(Guid authorId, Recipe recipe);

        Task<IList<Recipe>> GetByAuthorAsync(Guid authorId);

        Task<Recipe> UpdateAsync(Guid authorId, UpdateRecipeModel updatedRecipe);

        Task DeleteAsync(Guid authorId, Guid recipeId);
    }
}
