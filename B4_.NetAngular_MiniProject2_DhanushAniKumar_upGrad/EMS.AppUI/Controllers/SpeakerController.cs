using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.AppUI.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly ISpeakerRepository _repo;
        private readonly ISessionRepository _sessionRepo;

        public SpeakerController(ISpeakerRepository repo, ISessionRepository sessionRepo)
        {
            _repo = repo;
            _sessionRepo = sessionRepo;
        }

        public IActionResult Index()
        {
            var speakers = _repo.GetAll();
            return View(speakers);
        }

        [HttpPost]
        public IActionResult Create(SpeakersDetails model)
        {
            if (_repo.GetAll().Any(s => s.SpeakerName == model.SpeakerName))
            {
                ModelState.AddModelError("", "Speaker already exists");
            }

            if (ModelState.IsValid)
            {
                model.SpeakerId = Guid.NewGuid();
                _repo.Add(model);

                return RedirectToAction("Index");
            }

            return View("Index", _repo.GetAll());
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            var hasSessions = _sessionRepo.GetAll()
                .Any(s => s.SpeakerId == id);

            if (hasSessions)
            {
                TempData["Error"] = "Cannot delete speaker because it is assigned to sessions";
                return RedirectToAction("Index");
            }

            _repo.Delete(id);
            TempData["Message"] = "Speaker deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
