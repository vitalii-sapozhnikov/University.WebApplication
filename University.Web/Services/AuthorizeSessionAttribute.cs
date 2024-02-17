using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using University.Web.Services.Contracts;

namespace University.Web.Services
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeSessionAttribute : Attribute, IActionFilter
    {
        private readonly string[] allowedRoles;

        public AuthorizeSessionAttribute(params string[] roles)
        {
            allowedRoles = roles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionService = (ISessionService)context.HttpContext.RequestServices.GetService(typeof(ISessionService));

            if (!sessionService.isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }

            var (user, role) = sessionService.GetUser();

            if (allowedRoles.Length > 0)
            {
                if (!allowedRoles.Contains(role))
                    context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // You can add any post-processing logic here if needed
        }
    }
}
