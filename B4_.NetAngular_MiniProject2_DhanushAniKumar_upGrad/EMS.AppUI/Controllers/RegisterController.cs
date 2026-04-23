using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IParticipantRepository _repo;
        private readonly IEventRepository _eventRepo;

        public RegisterController(IParticipantRepository repo, IEventRepository eventRepo)
        {
            _repo = repo;
            _eventRepo = eventRepo;
        }

        public IActionResult Index(Guid eventId)
        {
            var ev = _eventRepo.GetById(eventId);
            return View(ev);
        }

        [HttpPost]
        public IActionResult Register(Guid eventId)
        {
            var user = HttpContext.Session.GetString("User");

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { eventId = eventId });
            }

            var alreadyRegistered = _repo.GetByUser(user)
                .Any(e => e.EventId == eventId);

            if (alreadyRegistered)
            {
                TempData["Message"] = "You already registered for this event";
                return RedirectToAction("Dashboard", "Account");
            }

            var data = new ParticipantEventDetails
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                ParticipantEmailId = user,
                IsAttended = false
            };

            _repo.RegisterEvent(data);

            return RedirectToAction("Dashboard", "Account");
        }

        public IActionResult Unregister(Guid eventId)
        {
            var user = HttpContext.Session.GetString("User");

            if (user != null)
            {
                _repo.RemoveRegistration(user, eventId);
            }

            return RedirectToAction("Dashboard", "Account");
        }
    }
}
