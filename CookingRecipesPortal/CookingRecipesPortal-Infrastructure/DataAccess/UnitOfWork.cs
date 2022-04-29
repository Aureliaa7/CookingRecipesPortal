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
        private readonly IRepository<UserRecipe> userRecipesRepository;

        public UnitOfWork(CookingRecipesPortalContext dbContext)
        {
            this.dbContext = dbContext;
            usersRepository = new Repository<User>(dbContext);
            recipesRepository = new Repository<Recipe>(dbContext);
            userRecipesRepository = new Repository<UserRecipe>(dbContext);
        }

        public IRepository<User> UsersRepository
        {
            get => usersRepository;
        }

        public IRepository<Recipe> RecipesRepository
        {
            get => recipesRepository;
        }

        public IRepository<UserRecipe> UserRecipesRepository
        {
            get => userRecipesRepository;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
