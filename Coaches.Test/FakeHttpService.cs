using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Coaches.Infrastructure;
using Coaches.MainApp.Services;

namespace Coaches.Test
{
    class FakeHttpService :BaseService, IHttpService
    {
        private Dictionary<string, Func<HttpResponseMessage>> _getEndpoints = new Dictionary<string, Func<HttpResponseMessage>>();
        private Dictionary<string, Func<HttpContent, HttpResponseMessage>> _postEndpoints = new Dictionary<string, Func<HttpContent, HttpResponseMessage>>();

        public void AssignGetEndpoint(string url, Func<HttpResponseMessage> func)
        {
            _getEndpoints[url] = func;
        }

        public void AssignPostEndpoint(string url, Func<HttpContent, HttpResponseMessage> func)
        {
            _postEndpoints[url] = func;
        }

        public void ClearGetEndpoints()
        {
            _getEndpoints.Clear();
        }

        public void ClearPostEndpoints()
        {
            _postEndpoints.Clear();
        }


        public ServiceResponse<HttpResponseMessage> Get(string url)
        {
           return TryExecute(() =>
            {
               if(!_getEndpoints.TryGetValue(url, out var func)) 
                   return ServiceResponse<HttpResponseMessage>.Error(new ErrorDetails(404, $"failed to find fake endpoints: {url}"));
               var httpResponse = func();
               return ServiceResponse.Success(httpResponse);
            });

        }

        public ServiceResponse<HttpResponseMessage> Post(string url, HttpContent content)
        {
            return TryExecute(() =>
            {
                if (!_postEndpoints.TryGetValue(url, out var func))
                    return ServiceResponse<HttpResponseMessage>.Error(new ErrorDetails(404, $"failed to find fake endpoints: {url}"));
                var httpResponse = func(content);
                return ServiceResponse.Success(httpResponse);
            });
        }
    }
}
