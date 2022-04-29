#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace CookingRecipesPortal_DAL.DomainModels
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string Steps { get; set; }

        public User Author { get; set; }

        public Guid AuthorId { get; set; }

        public DateTime PublishingDate { get; set; }
    }
}
