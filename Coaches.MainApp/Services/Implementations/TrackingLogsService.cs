using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Newtonsoft.Json;

namespace Coaches.MainApp.Services.Implementations
{
    public class TrackingLogsService : ITrackingLogsService
    {
        public ServiceResponse SendEvent(TrackingLogEvent trackingLogEvent)
        {
            using (var httpClient = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(trackingLogEvent); 
                var stringContent = new StringContent(jsonString);
                var result = httpClient.PostAsync("http://localhost:56103/Tracking/Event",stringContent).Result;
            }
            return ServiceResponse.Success();
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLatestLogs()
        {
            using (var httpClient = new HttpClient())
            {
                var result = httpClient.GetAsync("http://localhost:56103/Tracking/Logs").Result;
                var stringContent = result.Content.ReadAsStringAsync().Result;

                var trackingLogEvents = JsonConvert.DeserializeObject<List<TrackingLogEvent>>(stringContent);

                return ServiceResponse<List<TrackingLogEvent>>.Success(trackingLogEvents);
            }
            
        }
    }
}
