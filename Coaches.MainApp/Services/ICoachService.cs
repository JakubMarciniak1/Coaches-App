using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.Infrastructure;
using Coaches.MainApp.Models;

namespace Coaches.MainApp.Services
{
    public interface ICoachService
    {
        ServiceResponse<Coach> GetCoach(int id);
        ServiceResponse<List<Coach>> GetCoachList();
    }
}
