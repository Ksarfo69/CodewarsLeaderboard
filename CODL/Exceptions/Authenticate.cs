using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CODL.Exceptions;

public class Authenticate : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.HttpContext.Session.GetString("userId") == null)
        {
            filterContext.Result = new UnauthorizedResult();
        }
    }
}