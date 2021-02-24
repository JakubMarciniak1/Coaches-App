using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using Coaches.MainApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Coaches.MainApp.Controllers
{
    public class CoachController : BaseController
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
            if (IsErrorResponse(response, out var actionResult))
                return actionResult;
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
            var response = _coachService.AddCoach(coach);
            if (IsErrorResponse(response, out var actionResult))
                return actionResult;
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            EnsureCoachServiceInitialized();
            var response = _coachService.GetCoach(id);
            if (IsErrorResponse(response, out var actionResult))
                return actionResult;

            ViewData.Model = response.ResponseDTO;
            return View();
        }

        [HttpPost]
        public IActionResult Update(Coach coach)
        {
            EnsureCoachServiceInitialized();
            var response = _coachService.UpdateCoach(coach);
            if (IsErrorResponse(response, out var actionResult))
                return actionResult;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            EnsureCoachServiceInitialized();
            var response = _coachService.DeleteCoach(id);
            if (IsErrorResponse(response, out var actionResult))
                return actionResult;
        
            return RedirectToAction(nameof(Index));
        }

        private void EnsureCoachServiceInitialized()
        {
            _coachService.EnsureInitialized(Request);
        }
    }
}
