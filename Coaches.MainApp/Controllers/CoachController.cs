using Coaches.MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coaches.MainApp.Data;
using Coaches.MainApp.Services;
using Coaches.MainApp.Services.Implementations;

namespace Coaches.MainApp.Controllers
{
    public class CoachController : Controller
    {
        private readonly CoachesContext _context;
        private readonly ICoachService _coachService;

        public CoachController(CoachesContext context, ICoachService coachService)
        {
            _context = context;
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
            _context.Coach.Add(coach);
            _context.SaveChanges();
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
            _context.Coach.Update(coach);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Coach coach = _context.Coach.Find(id);
            if (coach != null)
            {
                _context.Coach.Remove(coach);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private List<Coach> SearchCoaches()
        {
            return _context.Coach.ToList();
        }
    }
}
