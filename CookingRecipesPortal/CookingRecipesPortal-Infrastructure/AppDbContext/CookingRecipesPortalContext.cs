using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_Infrastructure.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace CookingRecipesPortal_Infrastructure.AppDbContext
{
    public class CookingRecipesPortalContext : DbContext
    {
        public CookingRecipesPortalContext(DbContextOptions<CookingRecipesPortalContext> options) : base(options) { }

        public DbSet<User> AppUsers { get; set; }
        
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<LikedSavedRecipe> LikedSavedRecipes { get; set; }

        public DbSet<RecipeImage> RecipeImages { get; set; }

        public DbSet<FollowerFollowee> FollowerFollowees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new RecipeConfiguration().Configure(modelBuilder.Entity<Recipe>());
        }
    }
}
