namespace CookingRecipesPortal_DAL.DomainModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class RecipeImage
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }

        public string ImageName { get; set; }
    }
}
