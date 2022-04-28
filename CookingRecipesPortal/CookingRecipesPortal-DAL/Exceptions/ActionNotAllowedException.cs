namespace CookingRecipesPortal_DAL.Exceptions
{
    public class ActionNotAllowedException : Exception
    {
        public ActionNotAllowedException() : base() { }

        public ActionNotAllowedException(string message) : base(message) { }
    }
}
