using CookingRecipesPortal_DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CookingRecipesPortal_API.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is DuplicateEmailException)
            {
                context.Result = new ConflictResult();
            }
            else if (context.Exception is EntityNotFoundException)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
