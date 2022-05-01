using CookingRecipesPortal_DAL.Enums;

namespace CookingRecipesPortal_DAL.DomainModels
{
    public class LikedSavedRecipe
    {
        public Guid Id { get; set; }

        public Guid? RecipeId { get; set; }

        public Recipe? Recipe { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }

        public UserRecipeActionType ActionType { get; set; }

        public DateTime? SavingTime { get; set; }
    }
}
