using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.Tracking.Data;
using Microsoft.AspNetCore.Mvc;

namespace Coaches.Tracking.Services.Implementations
{
    public class TrackingService : BaseService, ITrackingService
    {

        private TrackingContext _trackingContext;

        public TrackingService(TrackingContext trackingContext)
        {
            _trackingContext = trackingContext;
        }

        public ServiceResponse SaveEvent(TrackingLogEvent trackingLogEvent)
        {
            return TryExecute(()=>
            {
                _trackingContext.TrackingLogEvent.Add(trackingLogEvent);
                _trackingContext.SaveChanges();
                return ServiceResponse.Success();
            });
           
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLogs()
        {
            return TryExecute(() =>
            {
                var logList = _trackingContext.TrackingLogEvent
                    .OrderByDescending(logEvent => logEvent.EventDate)
                    .Take(20)
                    .ToList();
                return ServiceResponse<List<TrackingLogEvent>>.Success(logList);
            });
        }

    }
}
