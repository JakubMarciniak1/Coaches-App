using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.Tracking.Data;
using Coaches.Tracking.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Coaches.Tracking.Services.Implementations
{
    public class TrackingService : BaseService, ITrackingService
    {

        private ITrackingLogEventRepository _repository;

        public TrackingService(ITrackingLogEventRepository repository)
        {
            _repository = repository;
        }

        public ServiceResponse SaveEvent(TrackingLogEvent trackingLogEvent)
        {
            return TryExecute(()=>
            {
                _repository.InsertEvent(trackingLogEvent);
                return ServiceResponse.Success();
            });
           
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLogs()
        {
            return TryExecute(() =>
            {
                var logList = _repository.GetEvents()
                    .OrderByDescending(logEvent => logEvent.EventDate)
                    .Take(20)
                    .ToList();
                return ServiceResponse<List<TrackingLogEvent>>.Success(logList);
            });
        }

    }
}
