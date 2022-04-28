using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Services
{
    public class CookingRecipeService : ICookingRecipeService
    {
        private readonly IUnitOfWork unitOfWork;

        public CookingRecipeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Recipe> AddAsync(Guid authorId, Recipe recipe)
        {
            await CheckUserExistenceAsync(authorId);

            recipe.AuthorId = authorId;
            var savedRecipe = await unitOfWork.RecipesRepository.AddAsync(recipe);
            await unitOfWork.SaveChangesAsync();

            return savedRecipe;
        }

        public async Task<IList<Recipe>> GetByAuthorAsync(Guid authorId)
        {
            await CheckUserExistenceAsync(authorId);

            var recipes = (await unitOfWork.RecipesRepository.GetAllAsync(
                x => x.AuthorId == authorId)).ToList();

            return recipes;
        }

        public async Task<Recipe> UpdateAsync(Guid authorId, UpdateRecipeModel updatedRecipe)
        {
            await CheckUserExistenceAsync(authorId);
            await CheckIfUserCanModifyRecipeAsync(authorId, updatedRecipe.Id);
            var recipe = await unitOfWork.RecipesRepository.GetByIdAsync(updatedRecipe.Id);
            if (recipe == null)
            {
                throw new EntityNotFoundException($"The recipe with id {updatedRecipe.Id} was not found!");
            }

            recipe.Name = updatedRecipe.Name;
            recipe.Description = updatedRecipe.Description;
            recipe.Steps = updatedRecipe.Steps;
            recipe.Ingredients = updatedRecipe.Ingredients;

            await unitOfWork.RecipesRepository.UpdateAsync(recipe);
            await unitOfWork.SaveChangesAsync();
            return recipe;
        }

        private async Task CheckIfUserCanModifyRecipeAsync(Guid authorId, Guid recipeId)
        {
            bool userCanUpdateRecipe = await unitOfWork.RecipesRepository.ExistsAsync(
               x => x.Id == recipeId && x.AuthorId == authorId);

            if (!userCanUpdateRecipe)
            {
                throw new ActionNotAllowedException("Only the user who posted a recipe can edit/delete it!");
            }
        }

        private async Task CheckUserExistenceAsync(Guid userId)
        {
            bool userExists = await unitOfWork.UsersRepository.ExistsAsync(x => x.Id == userId);
            if (!userExists)
            {
                throw new EntityNotFoundException($"The user with id {userId} was not found!");
            }
        }

        public async Task DeleteAsync(Guid authorId, Guid recipeId)
        {
            await CheckUserExistenceAsync(authorId);
            await CheckIfUserCanModifyRecipeAsync(authorId, recipeId);

            await unitOfWork.RecipesRepository.RemoveAsync(recipeId);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
