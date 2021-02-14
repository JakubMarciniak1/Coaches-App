using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Coaches.CommonModels;
using Coaches.Tracking.Services;

namespace Coaches.Tracking.Controllers
{
    public class TrackingController : Controller
    {
        private ITrackingService _trackingService;

        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpPost]
        public IActionResult Event([FromBody] TrackingLogEvent trackingLogEvent)
        {
            _trackingService.SaveEvent(trackingLogEvent);
            return Ok();
        }

        [HttpGet]
        public IActionResult Logs()
        {
            var response = _trackingService.GetLogs();
            return Ok(response.ResponseDTO);
        }

    }
}
