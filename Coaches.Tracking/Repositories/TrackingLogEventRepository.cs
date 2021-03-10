using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Tracking.Data;

namespace Coaches.Tracking.Repositories
{
    public class TrackingLogEventRepository : ITrackingLogEventRepository
    {

        private TrackingContext _trackingContext;

        public TrackingLogEventRepository(TrackingContext trackingContext)
        {
            _trackingContext = trackingContext;
        }

        public List<TrackingLogEvent> GetEvents()
        {
            return _trackingContext.TrackingLogEvent.ToList();
        }

        public TrackingLogEvent InsertEvent(TrackingLogEvent trackingLogEvent)
        {
            var entityEntry = _trackingContext.TrackingLogEvent.Add(trackingLogEvent);
            _trackingContext.SaveChanges();
            return entityEntry.Entity;
        }
    }
}
