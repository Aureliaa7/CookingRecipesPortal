using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Enums;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public class CookingRecipeService : ICookingRecipeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRecipeImageService imageService;
        private readonly IRecipePaginationService paginationService;
        private readonly ILikedSavedRecipePaginationService savedRecipePaginationService;

        public CookingRecipeService(IUnitOfWork unitOfWork, 
            IRecipeImageService imageService,
            IRecipePaginationService paginationService,
            ILikedSavedRecipePaginationService savedRecipePaginationService)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
            this.paginationService = paginationService;
            this.savedRecipePaginationService = savedRecipePaginationService;
        }
        public async Task<Recipe> AddAsync(Guid authorId, Recipe recipe, IList<string> base64Images)
        {
            await CheckUserExistenceAsync(authorId);

            recipe.AuthorId = authorId;
            recipe.PublishingDate = DateTime.Now;
            var savedRecipe = await unitOfWork.RecipesRepository.AddAsync(recipe);
            await unitOfWork.SaveChangesAsync();
            await imageService.SaveRecipeImagesAsync(base64Images, savedRecipe.Id);

            return savedRecipe;
        }

        public async Task<PagedResponseModel<RecipeModel>> GetByAuthorAsync(Guid authorId, PaginationFilter paginationFilter)
        {
            await CheckUserExistenceAsync(authorId);

            Expression<Func<Recipe, bool>> filter = x => x.AuthorId == authorId;

            var recipes = await paginationService.GetPagedResponseAsync(paginationFilter, filter);

            return recipes;
        }

        private async Task<int> GetNoLikesForRecipeAsync(Guid recipeId)
        {
            int noLikes = (await unitOfWork.LikedSavedRecipes.GetAllAsync(
                    x => x.RecipeId == recipeId &&
                    (x.ActionType == UserRecipeActionType.Like || x.ActionType == UserRecipeActionType.SaveAndLike)))
                    .Count();
            return noLikes;
        }

        public async Task<RecipeModel> UpdateAsync(Guid authorId, UpdateRecipeModel updatedRecipe)
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

            return new RecipeModel
            {
                Id = updatedRecipe.Id,
                Description = updatedRecipe.Description,
                Name = updatedRecipe.Name,
                Ingredients = updatedRecipe.Ingredients,
                Steps = updatedRecipe.Steps,
                AuthorId = authorId,
                NoLikes = await GetNoLikesForRecipeAsync(updatedRecipe.Id)
            };
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

        private async Task CheckRecipeExistenceAsync(Guid recipeId)
        {
            bool recipeExists = await unitOfWork.RecipesRepository.ExistsAsync(x => x.Id == recipeId);
            if (!recipeExists)
            {
                throw new EntityNotFoundException($"The recipe with id {recipeId} was not found!");
            }
        }

        public async Task DeleteAsync(Guid authorId, Guid recipeId)
        {
            await CheckRecipeExistenceAsync(recipeId);
            await CheckUserExistenceAsync(authorId);
            await CheckIfUserCanModifyRecipeAsync(authorId, recipeId);

            await imageService.DeleteRecipeImagesAsync(recipeId);

            var userRecipes = await unitOfWork.LikedSavedRecipes.GetAllAsync(x => x.RecipeId == recipeId);
            if (userRecipes.Any())
            {
                foreach (var userRecipe in userRecipes)
                {
                    await unitOfWork.LikedSavedRecipes.RemoveAsync(userRecipe);
                }
            }

            await unitOfWork.RecipesRepository.RemoveAsync(recipeId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SaveRecipeAsync(Guid userId, Guid recipeId)
        {
            await CheckIfUserRecipeEntityCanBeCreated(userId, recipeId, UserRecipeActionType.Save);

            // maybe the user liked the recipe but did not save it and now wants to save it
            var savedUserRecipe = await CreateOrUpdateUserRecipeEntityAsync(userId, recipeId, UserRecipeActionType.Save);
            savedUserRecipe.SavingTime = DateTime.Now;
            await unitOfWork.LikedSavedRecipes.UpdateAsync(savedUserRecipe);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task LikeRecipeAsync(Guid userId, Guid recipeId)
        {
            await CheckIfUserRecipeEntityCanBeCreated(userId, recipeId, UserRecipeActionType.Like);

            await CreateOrUpdateUserRecipeEntityAsync(userId, recipeId, UserRecipeActionType.Like);
        }

        private async Task CheckIfUserRecipeEntityCanBeCreated(Guid userId, Guid recipeId, UserRecipeActionType actionType)
        {
            bool entityExists = await unitOfWork.LikedSavedRecipes.ExistsAsync(
               x => x.UserId == userId &&
               x.RecipeId == recipeId &&
               (x.ActionType == actionType || x.ActionType == UserRecipeActionType.SaveAndLike));
            if (entityExists)
            {
                throw new DuplicateEntityException();
            }

            await CheckRecipeExistenceAsync(recipeId);
            await CheckUserExistenceAsync(userId);

            bool userIsRecipeAuthor = await unitOfWork.RecipesRepository.ExistsAsync(x => x.Id == recipeId && x.AuthorId == userId);
            if (userIsRecipeAuthor)
            {
                throw new ActionNotAllowedException("The author of a recipe cannot save their own recipes!");
            }
        }

        private async Task<LikedSavedRecipe> CreateOrUpdateUserRecipeEntityAsync(Guid userId, Guid recipeId, UserRecipeActionType actionType)
        {
            var existingRecipe = await unitOfWork.LikedSavedRecipes.GetFirstOrDefaultAsync(
               x => x.UserId == userId &&
               x.RecipeId == recipeId);

            if (existingRecipe != null)
            {
                existingRecipe.ActionType = UserRecipeActionType.SaveAndLike;
                await unitOfWork.LikedSavedRecipes.UpdateAsync(existingRecipe);
                await unitOfWork.SaveChangesAsync();
                return existingRecipe;
            }
            else
            {
                var userRecipe = new LikedSavedRecipe
                {
                    RecipeId = recipeId,
                    UserId = userId,
                    ActionType = actionType
                };

                await unitOfWork.LikedSavedRecipes.AddAsync(userRecipe);
                await unitOfWork.SaveChangesAsync();
                return userRecipe;
            }
        }

        public async Task RemoveFromSavedRecipesAsync(Guid userId, Guid recipeId)
        {
            await RemoveSpecificActionTypeFromUserRecipeAsync(userId, recipeId, UserRecipeActionType.Save);
            var userRecipe = await unitOfWork.LikedSavedRecipes.GetFirstOrDefaultAsync(
                x => x.UserId == userId && x.RecipeId == recipeId);
            if (userRecipe != null)
            {
                userRecipe.SavingTime = null;
                await unitOfWork.LikedSavedRecipes.UpdateAsync(userRecipe);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RemoveFromLikedRecipesAsync(Guid userId, Guid recipeId)
        {
            await RemoveSpecificActionTypeFromUserRecipeAsync(userId, recipeId, UserRecipeActionType.Like);
        }

        private async Task RemoveSpecificActionTypeFromUserRecipeAsync(Guid userId, Guid recipeId, UserRecipeActionType actionType)
        {
            bool updatedActionType = await UpdateRecipeActionTypeAsync(userId, recipeId, actionType);
            if (!updatedActionType)
            {
                var userRecipe = await unitOfWork.LikedSavedRecipes.GetFirstOrDefaultAsync(
                    x => x.UserId == userId && x.RecipeId == recipeId);
                await unitOfWork.LikedSavedRecipes.RemoveAsync(userRecipe);
                await unitOfWork.SaveChangesAsync();
            }
        }

        private async Task<bool> UpdateRecipeActionTypeAsync(Guid userId, Guid recipeId, UserRecipeActionType actionType)
        {
            var userRecipe = await unitOfWork.LikedSavedRecipes.GetFirstOrDefaultAsync(
                x => x.UserId == userId && x.RecipeId == recipeId &&
                (x.ActionType == actionType || x.ActionType == UserRecipeActionType.SaveAndLike));

            if (userRecipe == null)
            {
                throw new EntityNotFoundException($"UserRecipe with userId {userId} and recipeId {recipeId} was not found!");
            }

            if (userRecipe.ActionType == UserRecipeActionType.SaveAndLike)
            {
                userRecipe.ActionType = actionType == UserRecipeActionType.Save ? UserRecipeActionType.Like : UserRecipeActionType.Save;
                await unitOfWork.LikedSavedRecipes.UpdateAsync(userRecipe);
                await unitOfWork.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<PagedResponseModel<RecipeModel>> GetSavedRecipesAsync(Guid userId, PaginationFilter paginationFilter)
        {
            Expression<Func<LikedSavedRecipe, bool>> filter = x => x.UserId == userId && 
            (x.ActionType == UserRecipeActionType.Save || x.ActionType == UserRecipeActionType.SaveAndLike);

            var savedRecipesPagedResponse = await savedRecipePaginationService.GetPagedResponseAsync(paginationFilter, filter);
            var savedRecipesIds = savedRecipesPagedResponse.Data.Select(x => x.RecipeId).ToList();

            var recipes = (await unitOfWork.RecipesRepository.GetAllAsync(x => savedRecipesIds.Contains(x.Id))).ToList();
            var savedRecipes = new List<RecipeModel>();
            
            foreach (var recipe in recipes)
            {
                savedRecipes.Add(new RecipeModel
                {
                    AuthorId = recipe.Id,
                    Description = recipe.Description,
                    Id = recipe.Id,
                    Ingredients = recipe.Ingredients,
                    Name = recipe.Name,
                    PublishingDate = recipe.PublishingDate,
                    Steps = recipe.Steps,
                    Images = await imageService.GetRecipeImagesAsync(recipe.Id),
                    NoLikes = await GetNoLikesForRecipeAsync(recipe.Id)
                });
            }

            var pagedResponse = new PagedResponseModel<RecipeModel>
            {
                Data = savedRecipes,
                PageNumber = savedRecipesPagedResponse.PageNumber,
                PageSize = savedRecipesPagedResponse.PageSize,
                TotalPages = savedRecipesPagedResponse.TotalPages,
                TotalRecords = savedRecipesPagedResponse.TotalRecords
            };

            return pagedResponse;
        }
    }
}
