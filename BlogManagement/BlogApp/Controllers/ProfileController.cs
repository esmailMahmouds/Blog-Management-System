using BlogApp.Context;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(User user)
        {
            if (user.Email == null)
            {
                user= new User
                {
                    Name = "Esmail",
                    Email = "Esmail@gmail.com",
                    DateOfBirth = new DateOnly(2021, 8, 17),
                    Country = new Country { Id = 1, Name = "Egypt" }
                };
            }
            var countries = _context.Countries.ToList();
            ViewBag.countries = new SelectList(countries, "Id", "Name");

            ModelState.Clear();

            return View(user);
        }
        [HttpPost]
        public IActionResult EditUserInfo(User user)
        {
            Console.WriteLine(user.Name);
            return RedirectToAction("Index", user);
        }
    }
}
