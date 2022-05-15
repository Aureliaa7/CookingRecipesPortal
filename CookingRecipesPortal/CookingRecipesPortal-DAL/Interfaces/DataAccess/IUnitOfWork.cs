using CookingRecipesPortal_DAL.DomainModels;

namespace CookingRecipesPortal_DAL.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<User> UsersRepository { get; }

        IRepository<Recipe> RecipesRepository { get; }

        IRepository<LikedSavedRecipe> LikedSavedRecipesRepository { get; }

        IRepository<RecipeImage> RecipeImagesRepository { get; }

        IRepository<FollowerFollowee> FollowerFolloweesRepository { get; }

        Task SaveChangesAsync();
    }
}
