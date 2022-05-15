using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public class LikedSavedRecipePaginationService : PaginationServiceBase<LikedSavedRecipe, LikedSavedRecipe>, ILikedSavedRecipePaginationService
    {
        private readonly IUnitOfWork unitOfWork;

        public LikedSavedRecipePaginationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async override Task<PagedResponseModel<LikedSavedRecipe>> GetPagedResponseAsync(PaginationFilter paginationFilter, Expression<Func<LikedSavedRecipe, bool>>? filter = null)
        {
            int totalRecords = await unitOfWork.LikedSavedRecipes.GetTotalRecordsAsync(filter);

            var recipes = (await unitOfWork.LikedSavedRecipes.GetAllAsync(filter,
                skip: (paginationFilter.PageNumber - 1) * paginationFilter.PageSize,
                take: paginationFilter.PageSize))
                .ToList();

            return GetPagedResponseModel(recipes, totalRecords, paginationFilter);
        }
    }
}
