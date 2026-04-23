using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionRepository _sessionRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ISpeakerRepository _speakerRepo;

        public SessionController(ISessionRepository sessionRepo,
                                 IEventRepository eventRepo,
                                 ISpeakerRepository speakerRepo)
        {
            _sessionRepo = sessionRepo;
            _eventRepo = eventRepo;
            _speakerRepo = speakerRepo;
        }

        public IActionResult Index()
        {
            ViewBag.Events = _eventRepo.GetAll();
            ViewBag.Speakers = _speakerRepo.GetAll();

            var sessions = _sessionRepo.GetAll();
            return View(sessions);
        }

        [HttpPost]
        public IActionResult Create(SessionInfo model)
        {
            ModelState.Remove("Event");
            ModelState.Remove("Speaker");
            ModelState.Remove("SpeakerId");

            if (Request.Form["SpeakerId"] == "")
            {
                model.SpeakerId = null;
            }

            if (model.SessionStart >= model.SessionEnd)
            {
                ModelState.AddModelError("", "Start must be before End");
            }

            var ev = _eventRepo.GetAll()
                .FirstOrDefault(e => e.EventId == model.EventId);

            if (ev != null)
            {
                if (model.SessionStart.Date != ev.EventDate.Date)
                {
                    ModelState.AddModelError("", "Session must be on same date as Event");
                }
            }

            var isDuplicate = _sessionRepo.GetAll()
                .Any(s =>
                    s.EventId == model.EventId &&
                    s.SessionTitle.Trim().ToLower() == model.SessionTitle.Trim().ToLower()
                );

            if (isDuplicate)
            {
                ModelState.AddModelError("", "Session already exists for this event");
            }

            if (ModelState.IsValid)
            {
                model.SessionId = Guid.NewGuid();
                _sessionRepo.Add(model);

                return RedirectToAction("Index");
            }

            ViewBag.Events = _eventRepo.GetAll();
            ViewBag.Speakers = _speakerRepo.GetAll();

            return View("Index", _sessionRepo.GetAll());
        }

        public IActionResult Edit(Guid id)
        {
            var session = _sessionRepo.GetAll().FirstOrDefault(s => s.SessionId == id);

            ViewBag.Events = _eventRepo.GetAll();
            ViewBag.Speakers = _speakerRepo.GetAll();

            return View(session);
        }

        [HttpPost]
        public IActionResult Edit(SessionInfo model)
        {
            ModelState.Remove("Event");
            ModelState.Remove("Speaker");
            ModelState.Remove("SpeakerId");

            if (Request.Form["SpeakerId"] == "")
            {
                model.SpeakerId = null;
            }

            if (model.SessionStart >= model.SessionEnd)
            {
                ModelState.AddModelError("", "Start must be before End");
            }

            var ev = _eventRepo.GetAll()
                .FirstOrDefault(e => e.EventId == model.EventId);

            if (ev != null)
            {
                if (model.SessionStart.Date != ev.EventDate.Date)
                {
                    ModelState.AddModelError("", "Session must be on same date as Event");
                }
            }

            var isDuplicate = _sessionRepo.GetAll()
                .Any(s =>
                    s.EventId == model.EventId &&
                    s.SessionTitle.Trim().ToLower() == model.SessionTitle.Trim().ToLower() &&
                    s.SessionId != model.SessionId
                );

            if (isDuplicate)
            {
                ModelState.AddModelError("", "Session already exists for this event");
            }

            if (ModelState.IsValid)
            {
                _sessionRepo.Update(model);
                return RedirectToAction("Index");
            }

            ViewBag.Events = _eventRepo.GetAll();
            ViewBag.Speakers = _speakerRepo.GetAll();

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var session = _sessionRepo.GetAll().FirstOrDefault(s => s.SessionId == id);

            if (session != null)
            {
                _sessionRepo.Delete(id);
            }

            return RedirectToAction("Index");
        }


    }
}
