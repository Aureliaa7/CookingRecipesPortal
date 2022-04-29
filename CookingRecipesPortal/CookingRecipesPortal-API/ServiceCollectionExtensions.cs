using CookingRecipesPortal_API.Filters;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Services;
using CookingRecipesPortal_Infrastructure.AppDbContext;
using CookingRecipesPortal_Infrastructure.DataAccess;
using CookingRecipesPortal_Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CookingRecipesPortal_API
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICookingRecipeService, CookingRecipeService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRecipeImageService, RecipeImageService>();
        }

        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CookingRecipesPortalContext>(options =>
                    options.UseSqlServer(connectionString));
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, string key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void ConfigureGlobalFilters(this IServiceCollection services)
        {
            services.AddMvc(options => {
                options.Filters.Add(new GlobalExceptionFilter());
            });
        }
    }
}
