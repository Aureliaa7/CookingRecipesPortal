using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;

namespace CookingRecipesPortal_DAL.Services
{
    public class CookingRecipeService : ICookingRecipeService
    {
        private readonly IUnitOfWork unitOfWork;

        public CookingRecipeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Recipe> AddAsync(Guid userId, Recipe recipe)
        {
            await CheckUserExistenceAsync(userId);

            recipe.AuthorId = userId;
            var savedRecipe = await unitOfWork.RecipesRepository.AddAsync(recipe);
            await unitOfWork.SaveChangesAsync();

            return savedRecipe;
        }

        public async Task<IList<Recipe>> GetByUserAsync(Guid userId)
        {
            await CheckUserExistenceAsync(userId);

            var recipes = (await unitOfWork.RecipesRepository.GetAllAsync(
                x => x.AuthorId == userId)).ToList();

            return recipes;
        }

        private async Task CheckUserExistenceAsync(Guid userId)
        {
            bool userExists = await unitOfWork.UsersRepository.ExistsAsync(x => x.Id == userId);
            if (!userExists)
            {
                throw new EntityNotFoundException($"The user with id {userId} was not found!");
            }
        }
    }
}
