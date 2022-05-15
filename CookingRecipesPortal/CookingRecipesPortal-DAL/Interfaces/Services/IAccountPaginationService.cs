using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IAccountPaginationService : IPaginationService<UserModel, User>
    {
        Task<PagedResponseModel<UserModel>> GetFollowedAccountsAsync(
            Guid userId, PaginationFilter paginationFilter);
    }
}
