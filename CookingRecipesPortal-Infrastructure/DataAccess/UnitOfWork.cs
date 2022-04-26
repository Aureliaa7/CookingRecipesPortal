using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.Interfaces.DataAccess;
using CookingRecipesPortal_Infrastructure.AppDbContext;

namespace CookingRecipesPortal_Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CookingRecipesPortalContext dbContext;
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<Recipe> recipesRepository;

        public UnitOfWork(CookingRecipesPortalContext dbContext)
        {
            this.dbContext = dbContext;
            usersRepository = new Repository<User>(dbContext);
            recipesRepository = new Repository<Recipe>(dbContext);
        }

        public IRepository<User> UsersRepository
        {
            get => usersRepository;
        }

        public IRepository<Recipe> RecipesRepository
        {
            get => recipesRepository;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
