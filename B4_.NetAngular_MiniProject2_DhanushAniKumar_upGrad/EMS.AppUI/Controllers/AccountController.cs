using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly IParticipantRepository _participantRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ISessionRepository _sessionRepo;

        public AccountController(IUserRepository repo,
                                 IParticipantRepository participantRepo,
                                 IEventRepository eventRepo,
                                 ISessionRepository sessionRepo)
        {
            _repo = repo;
            _participantRepo = participantRepo;
            _eventRepo = eventRepo;
            _sessionRepo = sessionRepo;
        }

        // SIGNUP
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserInfo model)
        {
            ModelState.Remove("Role");

            model.Role = "Participant";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_repo.GetByEmail(model.EmailId) != null)
            {
                ModelState.AddModelError("", "Email already exists");
                return View(model);
            }

            _repo.Add(model);

            return RedirectToAction("Login");
        }

        // LOGIN
        public IActionResult Login(Guid? eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

       
        [HttpPost]
        public IActionResult Login(string email, string password, string role, Guid? eventId)
        {
            var user = _repo.GetByEmail(email.Trim());

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            if (user.Password.Trim() == password.Trim() && user.Role == role)
            {
                // PARTICIPANT
                if (role == "Participant")
                {
                    HttpContext.Session.SetString("User", email);

                    if (eventId != null)
                    {
                        return RedirectToAction("Index", "Register", new { eventId = eventId });
                    }

                    return RedirectToAction("Dashboard");
                }

                // ADMIN
                if (role == "Admin")
                {
                    HttpContext.Session.SetString("Admin", "true");
                    return RedirectToAction("Index", "Event");
                }
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            var user = HttpContext.Session.GetString("User");

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var registrations = _participantRepo.GetByUser(user);
            var events = _eventRepo.GetAll();
            var sessions = _sessionRepo.GetAll();

            var result = registrations.Select(r => new
            {
                Event = events.FirstOrDefault(e => e.EventId == r.EventId),
                Sessions = sessions.Where(s => s.EventId == r.EventId).ToList()
            }).ToList();

            return View(result);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
