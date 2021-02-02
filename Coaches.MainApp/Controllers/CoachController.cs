using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using Coaches.MainApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coaches.MainApp.Controllers
{
    public class CoachController : Controller
    {
        private readonly ICoachService _coachService;

        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        public IActionResult Index()
        {
            var response = _coachService.GetCoachList();
            ViewData.Model = response.ResponseDTO;
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Coach coach)
        {
            _coachService.AddCoach(coach);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var response = _coachService.GetCoach(id);
            ViewData.Model = response.ResponseDTO;
            return View();
        }

        [HttpPost]
        public IActionResult Update(Coach coach)
        {
            _coachService.UpdateCoach(coach);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _coachService.DeleteCoach(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
