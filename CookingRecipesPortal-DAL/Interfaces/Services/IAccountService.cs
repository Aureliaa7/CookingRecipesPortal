using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IAccountService
    {
        Task<string> LoginAsync(LoginDto loginDto);

        Task<User> RegisterAsync(User user);
    }
}
