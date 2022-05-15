using CookingRecipesPortal_DAL.DomainModels;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    //TODO find a better name
    public interface ILikedSavedRecipePaginationService : IPaginationService<LikedSavedRecipe, LikedSavedRecipe>
    {
    }
}
