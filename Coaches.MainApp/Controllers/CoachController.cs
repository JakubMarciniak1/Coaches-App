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
            EnsureCoachServiceInitialized();
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
            EnsureCoachServiceInitialized();
            _coachService.AddCoach(coach);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            EnsureCoachServiceInitialized();
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
            EnsureCoachServiceInitialized();
            _coachService.UpdateCoach(coach);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            EnsureCoachServiceInitialized();
            var response = _coachService.DeleteCoach(id);
            if (!response.IsSuccess)
            {
                TempData[nameof(ErrorDetails)] = JsonConvert.SerializeObject(response.ErrorDetails);
                return RedirectToAction(nameof(ErrorController.Message), "Error");
            }
            return RedirectToAction(nameof(Index));
        }

        private void EnsureCoachServiceInitialized()
        {
            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            _coachService.EnsureInitialized(url);
        }
    }
}
