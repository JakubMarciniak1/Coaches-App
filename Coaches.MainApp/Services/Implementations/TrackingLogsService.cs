using Coaches.CommonModels;
using Coaches.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Coaches.MainApp.Services.Implementations
{
    public class TrackingLogsService : BaseService, ITrackingLogsService
    {
        private readonly IConfiguration _configuration;

        public TrackingLogsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public ServiceResponse SendEvent(TrackingLogEvent trackingLogEvent)
        {
            return TryExecute(() =>
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonString = JsonConvert.SerializeObject(trackingLogEvent);
                    var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var url = _configuration.GetValue<string>("AppTrackingURLs:Event");
                    var response = httpClient.PostAsync(url, stringContent).Result;
                    if (!response.IsSuccessStatusCode)
                        return ServiceResponse.Error(new ErrorDetails((int)response.StatusCode, response.ReasonPhrase));
                }

                return ServiceResponse.Success();
            });
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLatestLogs()
        {
            return TryExecute(() =>
            {


                using (var httpClient = new HttpClient())
                {
                    var url = _configuration.GetValue<string>("AppTrackingURLs:Logs");
                    var response = httpClient.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode)
                        return ServiceResponse<List<TrackingLogEvent>>.Error(new ErrorDetails((int) response.StatusCode,
                            response.ReasonPhrase));
                    var stringContent = response.Content.ReadAsStringAsync().Result;

                    var trackingLogEvents = JsonConvert.DeserializeObject<List<TrackingLogEvent>>(stringContent);

                    return ServiceResponse<List<TrackingLogEvent>>.Success(trackingLogEvents);
                }
            });
        }
    }
}
