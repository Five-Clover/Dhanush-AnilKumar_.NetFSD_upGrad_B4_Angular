using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EMS.AppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventRepository _repo;

        public HomeController(IEventRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index(string search)
        {
            var events = _repo.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                events = events
                    .Where(e => e.EventName.Contains(search, StringComparison.OrdinalIgnoreCase)
                             || e.EventCategory.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(events);
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
