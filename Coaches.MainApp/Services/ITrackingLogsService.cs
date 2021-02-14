using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;

namespace Coaches.MainApp.Services
{
    public interface ITrackingLogsService
    {
        ServiceResponse SendEvent(TrackingLogEvent trackingLogEvent);
        ServiceResponse<List<TrackingLogEvent>> GetLatestLogs();
    }
}
