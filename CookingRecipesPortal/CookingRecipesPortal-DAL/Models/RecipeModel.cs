namespace CookingRecipesPortal_DAL.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class RecipeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string Steps { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public int NoLikes { get; set; }

        public DateTime PublishingDate { get; set; }

        public IList<byte[]> Images { get; set; }
    }
}
