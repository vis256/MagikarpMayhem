using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MagikarpMayhem.Attributes
{
    public class NotLoggedInAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "User", null);
            }
            base.OnActionExecuting(context);
        }
    }
}