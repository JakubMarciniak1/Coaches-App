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
        private readonly IHttpService _service;


        public TrackingLogsService(IConfiguration configuration, IHttpService service)
        {
            _configuration = configuration;
            _service = service;
        }


        public ServiceResponse SendEvent(TrackingLogEvent trackingLogEvent)
        {
            return TryExecute(() =>
            {

                var jsonString = JsonConvert.SerializeObject(trackingLogEvent);
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = _configuration.GetValue<string>("AppTrackingURLs:Event");
                var response = _service.Post(url, stringContent);
                if (!response.IsSuccess)
                    return response;

                var httpResponse = response.ResponseDTO;
                if (!httpResponse.IsSuccessStatusCode)
                    return ServiceResponse.Error(new ErrorDetails((int)httpResponse.StatusCode, httpResponse.ReasonPhrase));


                return ServiceResponse.Success();
            });
        }

        public ServiceResponse<List<TrackingLogEvent>> GetLatestLogs()
        {
            return TryExecute(() =>
            {
                var url = _configuration.GetValue<string>("AppTrackingURLs:Logs");
                var response = _service.Get(url);
                if (!response.IsSuccess)
                    return response.AsGenericResponse<List<TrackingLogEvent>>();

                var httpResponse = response.ResponseDTO;
                if (!httpResponse.IsSuccessStatusCode)
                    return ServiceResponse<List<TrackingLogEvent>>.Error(new ErrorDetails(
                        (int)httpResponse.StatusCode, httpResponse.ReasonPhrase));

                var stringContent = httpResponse.Content.ReadAsStringAsync().Result;
                var trackingLogEvents = JsonConvert.DeserializeObject<List<TrackingLogEvent>>(stringContent);
                return ServiceResponse<List<TrackingLogEvent>>.Success(trackingLogEvents);
            });
        }
    }
}
