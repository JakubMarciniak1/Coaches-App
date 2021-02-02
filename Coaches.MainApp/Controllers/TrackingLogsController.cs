using Coaches.MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Coaches.MainApp.Controllers
{
    public class TrackingLogsController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Model = SearchTrackingLogs();
            return View();
        }
        
        private List<TrackingLogEvent> SearchTrackingLogs()
        {
            throw new NotImplementedException();
        }


    }
}
