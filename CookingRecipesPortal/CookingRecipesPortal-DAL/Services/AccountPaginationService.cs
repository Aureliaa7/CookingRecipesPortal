using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public class AccountPaginationService: PaginationServiceBase<UserModel, User>, IAccountPaginationService
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountPaginationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<PagedResponseModel<UserModel>> GetFollowedAccountsAsync(
            Guid userId, PaginationFilter paginationFilter)
        {
            var followedUsersIds = (await unitOfWork.FollowerFolloweesRepository.GetAllAsync(
                x => x.FollowerId == userId))
                .Select(x => x.FolloweeId)
                .ToList();

            Expression<Func<User, bool>> filter = x => followedUsersIds.Contains(x.Id);
            return await GetUsersAsync(paginationFilter, filter);
        }

        public override Task<PagedResponseModel<UserModel>> GetPagedResponseAsync(
            PaginationFilter paginationFilter, 
            Expression<Func<User, bool>>? filter = null)
        {
            return GetUsersAsync(paginationFilter, filter);
        }

        private async Task<PagedResponseModel<UserModel>> GetUsersAsync(
            PaginationFilter paginationFilter,
            Expression<Func<User, bool>>? filter = null)
        {
            int totalRecords = await unitOfWork.UsersRepository.GetTotalRecordsAsync(filter);

            var users = (await unitOfWork.UsersRepository.GetAllAsync(filter,
                skip: (paginationFilter.PageNumber - 1) * paginationFilter.PageSize,
                take: paginationFilter.PageSize))
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                })
                .ToList();

            return GetPagedResponseModel(users, totalRecords, paginationFilter);
        }
    }
}
