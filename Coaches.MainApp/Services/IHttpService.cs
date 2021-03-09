using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Coaches.MainApp.Services
{
    public interface IHttpService
    { 
        ServiceResponse<HttpResponseMessage> Get(string url); 
        ServiceResponse<HttpResponseMessage> Post(string url, HttpContent content);
    }
}
