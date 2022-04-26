using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_Infrastructure.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipesPortal_Infrastructure.AppDbContext
{
    public class CookingRecipesPortalContext : DbContext
    {
        public CookingRecipesPortalContext(DbContextOptions<CookingRecipesPortalContext> options) : base(options) { }

        public DbSet<User> AppUsers { get; set; }
        
       public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new RecipeConfiguration().Configure(modelBuilder.Entity<Recipe>());
        }
    }
}
