using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> LoginAsync(LoginDto loginDto);

        Task<UserModel> RegisterAsync(User user);

        Task<PagedResponseModel<UserModel>> GetAccountsAsync(
            PaginationFilter paginationFilter, 
            Guid? excludeUserId = null);

        Task FollowAccountAsync(Guid followerId, Guid followeeId);

        Task UnfollowAccountAsync(Guid followerId, Guid followeeId);

        Task<PagedResponseModel<UserModel>> GetFolloweesAsync(Guid userId, PaginationFilter paginationFilter);

        Task<UserModel> GetAccountInfoAsync(Guid userId);
    }
}
