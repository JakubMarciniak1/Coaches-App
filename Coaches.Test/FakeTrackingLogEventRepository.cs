using Coaches.CommonModels;
using Coaches.Tracking.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Coaches.Test
{
    class FakeTrackingLogEventRepository : ITrackingLogEventRepository
    {
        private static int _nextId = 1;
        readonly Dictionary<int, TrackingLogEvent> _eventsDictionary = new Dictionary<int, TrackingLogEvent>();

        public List<TrackingLogEvent> GetEvents()
        {
            return _eventsDictionary.Values.ToList();
        }

        public TrackingLogEvent InsertEvent(TrackingLogEvent trackingLogEvent)
        {
            var dbEvent = new TrackingLogEvent()
            {
                CoachId =  trackingLogEvent.CoachId,
                EventDate = trackingLogEvent.EventDate,
                EventTypeId = trackingLogEvent.EventTypeId,
                UpdatePageUrl = trackingLogEvent.UpdatePageUrl,
                UserIPAddress = trackingLogEvent.UserIPAddress,
                Id = _nextId
            };
            _eventsDictionary[dbEvent.Id] = dbEvent;
            _nextId++;
            return dbEvent;
        }
    }
}
