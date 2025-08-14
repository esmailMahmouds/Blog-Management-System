using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters
{
    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            if (jwtService == null)
            {
                var controller = (Controller)context.Controller;
                controller.TempData["ErrorMessage"] = "Authentication service unavailable.";
                context.Result = new RedirectToActionResult("SignIn", "Account", null);
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
                var userId = jwtService.GetUserIdFromToken(jwtToken);
                if (userId <= 0)
                {
                    var controller = (Controller)context.Controller;
                    controller.TempData["ErrorMessage"] = "Invalid authentication token.";
                    context.Result = new RedirectToActionResult("SignIn", "Account", null);
                    return;
                }
            }
            catch
            {
                var controller = (Controller)context.Controller;
                controller.TempData["ErrorMessage"] = "Invalid or expired authentication token. Please sign in again.";
                context.Result = new RedirectToActionResult("SignIn", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}