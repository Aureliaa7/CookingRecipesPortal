using System.ComponentModel.DataAnnotations;

namespace CookingRecipesPortal_DAL.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class UpdateRecipeModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Steps { get; set; }
    }
}
