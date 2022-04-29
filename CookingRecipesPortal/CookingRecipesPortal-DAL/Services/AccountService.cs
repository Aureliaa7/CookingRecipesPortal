using CookingRecipesPortal_Core.Helpers;
using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using System.Text;

namespace CookingRecipesPortal_DAL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtService jwtService;

        public AccountService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
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

        public async Task<User> RegisterAsync(User user)
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

            // do not return password related data
            // TODO create another model and exclude password related data
            newUser.PasswordHash = Encoding.UTF8.GetBytes(string.Empty);
            newUser.PasswordSalt = Encoding.UTF8.GetBytes(string.Empty);
            newUser.Password = string.Empty;

            return newUser;
        }
    }
}
