using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using Coaches.MainApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            if (!response.IsSuccess)
            {
                TempData[nameof(ErrorDetails)] = JsonConvert.SerializeObject(response.ErrorDetails);
                return RedirectToAction(nameof(ErrorController.Message), "Error");
            }
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
            var response = _coachService.DeleteCoach(id);
            if (!response.IsSuccess)
            {
                TempData[nameof(ErrorDetails)] = JsonConvert.SerializeObject(response.ErrorDetails);
                return RedirectToAction(nameof(ErrorController.Message), "Error");
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
