using CookingRecipesPortal_DAL.DomainModels;

namespace CookingRecipesPortal_DAL.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
        IRepository<User> UsersRepository { get; }

        IRepository<Recipe> RecipesRepository { get; }

        Task SaveChangesAsync();
    }
}
