using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _repo;

        public AdminController(IUserRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _repo.GetByEmail(email);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            if (user.Password == password && user.Role == "Admin")
            {
                HttpContext.Session.SetString("Admin", "true");
                return RedirectToAction("Index", "Event");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Account");
        }
    }
}
