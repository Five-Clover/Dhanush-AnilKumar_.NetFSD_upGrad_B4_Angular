using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _repo;
        private readonly ISessionRepository _sessionRepo;

        public EventController(IEventRepository repo, ISessionRepository sessionRepo)
        {
            _repo = repo;
            _sessionRepo = sessionRepo;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") != "true")
            {
                return RedirectToAction("Login", "Admin");
            }

            var events = _repo.GetAll();
            return View(events);
        }

        [HttpPost]
        public IActionResult Create(EventDetails model)
        {
            if (string.IsNullOrWhiteSpace(model.EventName))
            {
                ModelState.AddModelError("", "Event name cannot be empty or spaces");
            }
            else if (_repo.GetAll().Any(e =>
                    e != null &&
                    !string.IsNullOrWhiteSpace(e.EventName) &&
                    e.EventName.Trim().ToLower() == model.EventName.Trim().ToLower()))
                {
                    ModelState.AddModelError("", "Event already exists");
                }
            

            if (ModelState.IsValid)
            {
                model.EventId = Guid.NewGuid();
                _repo.Add(model);

                return RedirectToAction("Index");
            }

            return View("Index", _repo.GetAll());
        }

        public IActionResult Edit(Guid id)
        {
            var ev = _repo.GetById(id);

            if (ev == null)
            {
                TempData["Error"] = "Event not found";
                return RedirectToAction("Index");
            }

            return View(ev);
        }

        [HttpPost]
        public IActionResult Edit(EventDetails model)
        {
            if (string.IsNullOrWhiteSpace(model.EventName))
            {
                ModelState.AddModelError("", "Event name cannot be empty or spaces");
            }
            else if (_repo.GetAll().Any(e =>
                    e.EventId != model.EventId &&
                    !string.IsNullOrWhiteSpace(e.EventName) &&
                    e.EventName.Trim().ToLower() == model.EventName.Trim().ToLower()))
            {
                ModelState.AddModelError("", "Event already exists");
            }

            if (ModelState.IsValid)
            {
                _repo.Update(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var ev = _repo.GetById(id);

            if (ev == null)
            {
                TempData["Error"] = "Event not found";
                return RedirectToAction("Index");
            }

            var hasSessions = _sessionRepo.GetAll()
                .Any(s => s.EventId == id);

            if (hasSessions)
            {
                TempData["Error"] = "Cannot delete event because it has sessions";
                return RedirectToAction("Index");
            }

            _repo.Delete(id);
            TempData["Message"] = "Event deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
