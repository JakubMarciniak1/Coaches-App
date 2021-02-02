using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coaches.MainApp.Models
{
    public class TrackingLogEvent
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public TrackingLogEventType EventType { get; set; }
        public string UserIPAddress { get; set; }
        public int CoachId { get; set; }
        public string UpdatePageUrl { get; set; }
    }
}
