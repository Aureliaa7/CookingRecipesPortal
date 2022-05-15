using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface ICookingRecipeService
    {
        Task<Recipe> AddAsync(Guid authorId, Recipe recipe, IList<string> base64Images);

        Task<PagedResponseModel<RecipeModel>> GetByAuthorAsync(Guid authorId, PaginationFilter paginationFilter);

        Task<RecipeModel> UpdateAsync(Guid authorId, UpdateRecipeModel updatedRecipe);

        Task DeleteAsync(Guid authorId, Guid recipeId);

        Task SaveRecipeAsync(Guid userId, Guid recipeId);

        Task LikeRecipeAsync(Guid userId, Guid recipeId);

        Task RemoveFromSavedRecipesAsync(Guid userId, Guid recipeId);

        Task RemoveFromLikedRecipesAsync(Guid userId, Guid recipeId);

        Task<PagedResponseModel<RecipeModel>> GetSavedRecipesAsync(Guid userId, PaginationFilter paginationFilter);
    }
}
