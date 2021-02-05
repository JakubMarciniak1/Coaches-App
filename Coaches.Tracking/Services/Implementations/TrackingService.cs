﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.Tracking.Data;

namespace Coaches.Tracking.Services.Implementations
{
    public class TrackingService :ITrackingService
    {

        private TrackingContext _trackingContext;

        public TrackingService(TrackingContext trackingContext)
        {
            _trackingContext = trackingContext;
        }

        public ServiceResponse SaveEvent(TrackingLogEvent trackingLogEvent)
        {
            _trackingContext.TrackingLogEvent.Add(trackingLogEvent);
            return ServiceResponse.Success();
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLogs()
        {
            var logList = _trackingContext.TrackingLogEvent
                .OrderByDescending(logEvent => logEvent.EventDate)
                .Take(20)
                .ToList();

            return ServiceResponse<List<TrackingLogEvent>>.Success(logList);
        }

    }
}
