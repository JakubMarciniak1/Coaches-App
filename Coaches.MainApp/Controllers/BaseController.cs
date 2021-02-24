using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Coaches.MainApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected bool IsErrorResponse(ServiceResponse serviceResponse, out IActionResult actionResult)
        {
            if (!serviceResponse.IsSuccess)
            {
                TempData[nameof(ErrorDetails)] = JsonConvert.SerializeObject(serviceResponse.ErrorDetails);
                actionResult = RedirectToAction(nameof(ErrorController.Message), "Error");
                return true;
            }

            actionResult = null;
            return false;
        }
    }
}
