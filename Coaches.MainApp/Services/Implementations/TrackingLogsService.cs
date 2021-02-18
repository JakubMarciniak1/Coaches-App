using Coaches.CommonModels;
using Coaches.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Coaches.MainApp.Services.Implementations
{
    public class TrackingLogsService : ITrackingLogsService
    {
        private readonly IConfiguration _configuration;

        public TrackingLogsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ServiceResponse SendEvent(TrackingLogEvent trackingLogEvent)
        {
            trackingLogEvent.UserIPAddress = "123";
            using (var httpClient = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(trackingLogEvent); 
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = _configuration.GetValue<string>("AppTrackingURLs:Event");
                var result = httpClient.PostAsync(url,stringContent).Result;
            }
            return ServiceResponse.Success();
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLatestLogs()
        {
            using (var httpClient = new HttpClient())
            {
                var url = _configuration.GetValue<string>("AppTrackingURLs:Logs");
                var result = httpClient.GetAsync(url).Result;
                var stringContent = result.Content.ReadAsStringAsync().Result;

                var trackingLogEvents = JsonConvert.DeserializeObject<List<TrackingLogEvent>>(stringContent);

                return ServiceResponse<List<TrackingLogEvent>>.Success(trackingLogEvents);
            }
            
        }
    }
}
