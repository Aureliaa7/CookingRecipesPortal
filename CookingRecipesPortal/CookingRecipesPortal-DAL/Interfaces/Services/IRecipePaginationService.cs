using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Models;

namespace CookingRecipesPortal_DAL.Interfaces.Services
{
    public interface IRecipePaginationService: IPaginationService<RecipeModel, Recipe>
    {
    }
}
