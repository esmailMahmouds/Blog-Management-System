using BlogApp.Enums;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            if (jwtService == null)
            {
                context.Result = new RedirectToActionResult("Index", "Landing", null);
                return;
            }

            context.HttpContext.Request.Cookies.TryGetValue("Jwt", out string? jwtToken);

            if (string.IsNullOrEmpty(jwtToken))
            {
                var controller = (Controller)context.Controller;
                controller.TempData["ErrorMessage"] = "Please sign in to access this page.";
                context.Result = new RedirectToActionResult("SignIn", "Account", null);
                return;
            }

            try
            {
                Role userRole = jwtService.GetUserRoleFromToken(jwtToken);
                if (userRole != Role.Admin)
                {
                    var controller = (Controller)context.Controller;
                    controller.TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                    context.Result = new RedirectToActionResult("Index", "Landing", null);
                    return;
                }
            }
            catch
            {
                var controller = (Controller)context.Controller;
                controller.TempData["ErrorMessage"] = "Invalid authentication token.";
                context.Result = new RedirectToActionResult("SignIn", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}