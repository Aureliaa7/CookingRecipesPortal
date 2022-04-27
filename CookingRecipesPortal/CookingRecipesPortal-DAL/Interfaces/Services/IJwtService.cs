using CookingRecipesPortal_DAL.DTOs;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(LoginDto loginDto);
    }
}
