using CookingRecipesPortal_Core.Helpers;
using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using System.Linq.Expressions;

namespace CookingRecipesPortal_DAL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtService jwtService;
        private readonly IAccountPaginationService accountPaginationService;

        public AccountService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IAccountPaginationService accountPaginationService)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.accountPaginationService = accountPaginationService;
        }

        public async Task FollowAccountAsync(Guid followerId, Guid followeeId)
        {
            await CheckIfUserExistsAsync(followerId);
            await CheckIfUserExistsAsync(followeeId);

            var entityExists = await unitOfWork.FollowerFolloweesRepository.ExistsAsync(
                x => x.FollowerId == followerId && x.FolloweeId == followeeId);
            if (entityExists)
            {
                throw new DuplicateEntityException($"User with the id {followerId} already follows user with id {followeeId}!");
            }

            await unitOfWork.FollowerFolloweesRepository.AddAsync(new FollowerFollowee
            {
                FollowerId = followerId,
                FolloweeId = followeeId
            });
            await unitOfWork.SaveChangesAsync();
        }

        private async Task CheckIfUserExistsAsync(Guid userId)
        {
            bool userExists = await unitOfWork.UsersRepository.ExistsAsync(x => x.Id == userId);
            if (!userExists)
            {
                throw new EntityNotFoundException($"The user with the id {userId} does not exist!");
            }
        }

        public async Task<UserModel> GetAccountInfoAsync(Guid userId)
        {
            await CheckIfUserExistsAsync(userId);
            var user = await unitOfWork.UsersRepository.GetFirstOrDefaultAsync(x => x.Id == userId);
            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public Task<PagedResponseModel<UserModel>> GetAccountsAsync(
            PaginationFilter paginationFilter, Guid? excludeUserId = null)
        {
            Expression<Func<User, bool>> filter = x => x.Id != excludeUserId;
            return accountPaginationService.GetPagedResponseAsync(paginationFilter, filter);
        }

        public async Task<PagedResponseModel<UserModel>> GetFolloweesAsync(Guid userId, PaginationFilter paginationFilter)
        {
            await CheckIfUserExistsAsync(userId);
            return await accountPaginationService.GetFollowedAccountsAsync(userId, paginationFilter);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            string token = string.Empty;
            try
            {
                token = await jwtService.GenerateTokenAsync(loginDto);
            }
            catch (EntityNotFoundException) { }

            return token;
        }

        public async Task<UserModel> RegisterAsync(User user)
        {
            bool userExists = await unitOfWork.UsersRepository.ExistsAsync(u => u.Email.Equals(user.Email));
            if (userExists)
            {
                throw new DuplicateEntityException("A user with the same email already exists!");
            }

            PasswordHelper.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var newUser = await unitOfWork.UsersRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            var savedUser = new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id
            };

            return savedUser;
        }

        public async Task UnfollowAccountAsync(Guid followerId, Guid followeeId)
        {
            var entityId = await unitOfWork.FollowerFolloweesRepository.GetFirstOrDefaultAsync(
                x => x.FollowerId == followerId && x.FolloweeId == followeeId);
            if (entityId == null)
            {
                throw new EntityNotFoundException();
            }
            
            await unitOfWork.FollowerFolloweesRepository.RemoveAsync(entityId);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
