using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Enums;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public class RecipePaginationService : PaginationServiceBase<RecipeModel, Recipe>, IRecipePaginationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRecipeImageService imageService;

        public RecipePaginationService(IUnitOfWork unitOfWork, IRecipeImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
        }

        public override Task<PagedResponseModel<RecipeModel>> GetPagedResponseAsync(
            PaginationFilter paginationFilter, 
            Expression<Func<Recipe, bool>>? filter = null)
        {
            return GetPagedRecipesModelsAsync(paginationFilter, filter);  
        }

        private async Task<List<Recipe>> GetRecipesAsync(
            PaginationFilter paginationFilter, 
            Expression<Func<Recipe, bool>>? filter = null)
        {
            var recipes = (await unitOfWork.RecipesRepository.GetAllAsync(filter,
                skip: (paginationFilter.PageNumber - 1) * paginationFilter.PageSize,
                take: paginationFilter.PageSize))
                .ToList();

            return recipes;
        }

        private async Task<PagedResponseModel<RecipeModel>> GetPagedRecipesModelsAsync(
            PaginationFilter paginationFilter, Expression<Func<Recipe, bool>>? filter = null)
        {
            int totalRecords = await unitOfWork.RecipesRepository.GetTotalRecordsAsync(filter);
            var recipes = await GetRecipesAsync(paginationFilter, filter);
            var recipeModels = await GetRecipeModelsAsync(recipes);

            return GetPagedResponseModel(recipeModels, totalRecords, paginationFilter);
        }

        private async Task<List<RecipeModel>> GetRecipeModelsAsync(List<Recipe> recipes)
        {
            var recipeModels = new List<RecipeModel>();

            foreach (var recipe in recipes)
            {
                var author = await unitOfWork.UsersRepository.GetByIdAsync(recipe.AuthorId);
                recipeModels.Add(new RecipeModel
                {
                    Id = recipe.Id,
                    AuthorId = recipe.AuthorId,
                    AuthorName = $"{author?.FirstName} {author?.LastName}",
                    Description = recipe.Description,
                    Ingredients = recipe.Ingredients,
                    Name = recipe.Name,
                    NoLikes = await GetNoLikesForRecipeAsync(recipe.Id),
                    PublishingDate = recipe.PublishingDate,
                    Steps = recipe.Steps,
                    Images = await imageService.GetRecipeImagesAsync(recipe.Id)
                });
            }

            return recipeModels;
        }

        private async Task<int> GetNoLikesForRecipeAsync(Guid recipeId)
        {
            int noLikes = (await unitOfWork.LikedSavedRecipesRepository.GetAllAsync(
                    x => x.RecipeId == recipeId &&
                    (x.ActionType == UserRecipeActionType.Like || x.ActionType == UserRecipeActionType.SaveAndLike)))
                    .Count();
            return noLikes;
        }

        public async Task<PagedResponseModel<RecipeModel>> GetSavedRecipesAsync(
            Guid userId, PaginationFilter paginationFilter)
        {
            Expression<Func<LikedSavedRecipe, bool>> savedRecipesFilter = x => x.UserId == userId &&
            (x.ActionType == UserRecipeActionType.Save || x.ActionType == UserRecipeActionType.SaveAndLike);

            var savedRecipesIds = (await unitOfWork.LikedSavedRecipesRepository.GetAllAsync(
                savedRecipesFilter))
                .OrderByDescending(x => x.SavingTime)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .Select(x => x.RecipeId)
                .ToList();

            int totalRecords = await unitOfWork.LikedSavedRecipesRepository.GetTotalRecordsAsync(savedRecipesFilter);

            Expression<Func<Recipe, bool>> recipesFilter = x => savedRecipesIds.Contains(x.Id);

            var pagedResponse = await GetPagedRecipesModelsAsync(paginationFilter, recipesFilter);
            pagedResponse.Data = pagedResponse.Data.OrderBy(x => savedRecipesIds.IndexOf(x.Id)).ToList();
            return pagedResponse;
        }


        // Hence I cannot return PagedResponse<ExtendedRecipeModel> from here,
        // return PagedResponse<RecipeModel> and convert it toPagedResponse<ExtendedRecipeModel> in CookingRecipeService
        public async Task<PagedResponseModel<RecipeModel>> GetFollowedUsersRecipesAsync(Guid userId, PaginationFilter paginationFilter)
        {
            var followeesIds = await GetFollowedUsersIdsAsync(userId);
            var recipesIds = (await unitOfWork.RecipesRepository.GetAllAsync(
                x => followeesIds.Contains(x.AuthorId)))
                .OrderByDescending(x => x.PublishingDate)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .Select(x => x.Id)
                .ToList();

            Expression<Func<Recipe, bool>> recipesFilter = x => recipesIds.Contains(x.Id);
            var pagedResponse = await GetPagedRecipesModelsAsync(paginationFilter, recipesFilter);
            pagedResponse.Data = pagedResponse.Data.OrderBy(x => recipesIds.IndexOf(x.Id)).ToList();

            return pagedResponse;
        }

        private async Task<List<Guid>> GetFollowedUsersIdsAsync(Guid userId)
        {
            var usersIds = (await unitOfWork.FollowerFolloweesRepository
                .GetAllAsync(x => x.FollowerId == userId))
                .Select(x => x.FolloweeId)
                .ToList();

            return usersIds;
        }
    }
}
