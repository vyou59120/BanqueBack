using BanqueBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BanqueBack.Helpers
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public IList<string> _roles;

        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? new string[] { };
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var task = (Task<ActionResult<Login>>)context.HttpContext.Items["User"];
            var user = task.Result.Value;

            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
