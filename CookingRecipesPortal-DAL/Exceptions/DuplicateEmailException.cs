using System;
namespace CookingRecipesPortal_DAL.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException() : base() { }

        public DuplicateEmailException(string message) : base(message) { }
    }
}
