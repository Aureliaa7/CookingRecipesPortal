namespace CookingRecipesPortal_DAL.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException() : base() { }

        public DuplicateEntityException(string message) : base(message) { }
    }
}
