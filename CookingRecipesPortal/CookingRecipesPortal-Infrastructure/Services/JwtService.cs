using CookingRecipesPortal_Core.Helpers;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Exceptions;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CookingRecipesPortal_Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public JwtService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(LoginDto loginDto)
        {
            var user = await unitOfWork.UsersRepository.GetFirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                throw new EntityNotFoundException("The user was not found!");
            }

            bool isCorrectPassword = PasswordHelper.IsCorrectPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isCorrectPassword)
            {
                throw new EntityNotFoundException("Wrong credentials!");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            string jwtKey = configuration.GetSection("JWTKey").Value;
            var tokenKeyBytes = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, loginDto.Email),
                    new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
