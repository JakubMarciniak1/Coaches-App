using Coaches.MainApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coaches.MainApp.Controllers
{
    public class TrackingLogsController : Controller
    {
        private readonly ITrackingLogsService _trackingLogsService;

        public TrackingLogsController(ITrackingLogsService trackingLogsService)
        {
            _trackingLogsService = trackingLogsService;
        }

        public IActionResult Index()
        {
            var response = _trackingLogsService.GetLatestLogs();
            ViewData.Model = response.ResponseDTO;
            return View();
        }

    }
}
