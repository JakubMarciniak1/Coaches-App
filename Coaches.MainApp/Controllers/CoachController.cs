using Coaches.MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coaches.MainApp.Data;

namespace Coaches.MainApp.Controllers
{
    public class CoachController : Controller
    {
        private readonly CoachesContext _context;

        private readonly ILogger<CoachController> _logger;

        public CoachController(ILogger<CoachController> logger, CoachesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData.Model = SearchCoaches();
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit()
        {
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Coach> SearchCoaches()
        {
            return _context.Coach.ToList();
        }
    }
}
