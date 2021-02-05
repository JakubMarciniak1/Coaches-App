using System.Collections.Generic;
using Coaches.CommonModels;
using Coaches.Infrastructure;

namespace Coaches.Tracking.Services
{
    public interface ITrackingService
    {
         ServiceResponse SaveEvent(TrackingLogEvent trackingLogEvent);
        ServiceResponse<List<TrackingLogEvent>> GetLogs();
    }
}
