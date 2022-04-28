using AutoMapper;
using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;

namespace CookingRecipesPortal_Infrastructure.MappingConfigurations
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateUserMappings();

            CreateMap<Recipe, RecipeDto>().ReverseMap();
        }

        private void CreateUserMappings()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
