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
        private readonly IRepository<LikedSavedRecipe> likedSavedRecipesRepository;
        private readonly IRepository<RecipeImage> recipeImagesRepository;
        private readonly IRepository<FollowerFollowee> followerFolloweeRepository;

        public UnitOfWork(CookingRecipesPortalContext dbContext)
        {
            this.dbContext = dbContext;
            usersRepository = new Repository<User>(dbContext);
            recipesRepository = new Repository<Recipe>(dbContext);
            likedSavedRecipesRepository = new Repository<LikedSavedRecipe>(dbContext);
            recipeImagesRepository = new Repository<RecipeImage>(dbContext);
            followerFolloweeRepository = new Repository<FollowerFollowee>(dbContext);
        }

        public IRepository<User> UsersRepository
        {
            get => usersRepository;
        }

        public IRepository<Recipe> RecipesRepository
        {
            get => recipesRepository;
        }

        public IRepository<LikedSavedRecipe> LikedSavedRecipesRepository
        {
            get => likedSavedRecipesRepository;
        }

        public IRepository<RecipeImage> RecipeImagesRepository
        {
            get => recipeImagesRepository;
        }

        public IRepository<FollowerFollowee> FollowerFolloweesRepository
        {
            get => followerFolloweeRepository;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
