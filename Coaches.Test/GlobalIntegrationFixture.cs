using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Coaches.CommonModels;
using Coaches.Tracking.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Coaches.Test
{
    class GlobalIntegrationFixture : IDisposable
    {
        private TrackingController _trackingController;
        public FakeHttpService FakeHttpService { get; private set; }

        public GlobalIntegrationFixture()
        {
            FakeHttpService = new FakeHttpService();
            FakeHttpService.AssignGetEndpoint("http://localhost:56103/Tracking/Logs", FakeGetCoachList);
            FakeHttpService.AssignPostEndpoint("http://localhost:56103/Tracking/Event", FakeSendEvent);
        }

        public void InitTrackingController(TrackingController trackingController)
        {
            _trackingController = trackingController;
        }

        private HttpResponseMessage FakeGetCoachList()
        {
            var actionResult = _trackingController.Logs();
            var response = new HttpResponseMessage();
            if (actionResult is ObjectResult result)
            {
                if (result.StatusCode.HasValue)
                    response.StatusCode = (HttpStatusCode)result.StatusCode.Value;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = JsonConvert.SerializeObject(result.Value);
                    response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                }
            }

            return response;
        }

        private HttpResponseMessage FakeSendEvent(HttpContent content)
        {
            var jsonString = content.ReadAsStringAsync().Result;
            var trackingLogEvent = JsonConvert.DeserializeObject<TrackingLogEvent>(jsonString);
            var actionResult = _trackingController.Event(trackingLogEvent);

            var response = new HttpResponseMessage();
            if (actionResult is StatusCodeResult result)
                response.StatusCode = (HttpStatusCode)result.StatusCode;
            else if (actionResult is ObjectResult result2)
            {
                if (result2.StatusCode.HasValue)
                    response.StatusCode = (HttpStatusCode)result2.StatusCode.Value;
            }

            return response;

        }

        public void Dispose()
        {
            FakeHttpService.ClearGetEndpoints();
            FakeHttpService.ClearPostEndpoints();
        }
    }
}
