namespace CookingRecipesPortal_DAL.Models
{
    // Model used for retrieving saved recipes and recipes of followed users
    public class ExtendedRecipeModel : RecipeModel
    {
        public bool IsSaved { get; set; }

        public bool IsLiked { get; set; }
    }
}
