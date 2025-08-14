using BlogApp.Enums;
using BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Controllers
{
    public abstract class BaseAdminController : Controller
    {
        protected readonly IJwtService _jwtService;

        protected BaseAdminController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                context.Result = RedirectToAction("Index", "Home");
                return;
            }

            base.OnActionExecuting(context);
        }

        protected bool IsAdmin()
        {
            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            if (string.IsNullOrEmpty(jwtToken))
                return false;

            try
            {
                Role userRole = _jwtService.GetUserRoleFromToken(jwtToken);
                return userRole == Role.Admin;
            }
            catch
            {
                return false;
            }
        }

        protected int GetCurrentUserId()
        {
            Request.Cookies.TryGetValue("Jwt", out string? jwtToken);
            if (string.IsNullOrEmpty(jwtToken))
                throw new UnauthorizedAccessException("User not authenticated");

            try
            {
                return _jwtService.GetUserIdFromToken(jwtToken);
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid authentication token");
            }
        }
    }
}