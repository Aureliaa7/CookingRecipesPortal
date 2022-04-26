using System.Collections.ObjectModel;

namespace CookingRecipesPortal_DAL.DomainModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Password { get; set; }

        public ICollection<Recipe> Recipes { get; set; } = new Collection<Recipe>();
    }
}
