using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            if (Request.Cookies.ContainsKey("Jwt") && !string.IsNullOrEmpty(Request.Cookies["Jwt"]))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}