using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.Infrastructure;
using Coaches.MainApp.Data;
using Coaches.MainApp.Models;

namespace Coaches.MainApp.Services.Implementations
{
    public class CoachService : ICoachService
    {
        private CoachesContext _coachesContext;

        public CoachService(CoachesContext coachesContext)
        {
            _coachesContext = coachesContext;
        }

        public ServiceResponse<List<Coach>> GetCoachList()
        {
            var coachesList = _coachesContext.Coach.ToList();
            return ServiceResponse<List<Coach>>.Success(coachesList);
        }

       public ServiceResponse<Coach> GetCoach(int id)
        {
            var coach = _coachesContext.Coach.Find(id);
            //if (coach == null)
            //{
            //    return ServiceResponse.Error(new ErrorDetails(404, $"Could not find coach(ID {id})"));
            //}
            return ServiceResponse<Coach>.Success(coach);
        }
    }
}
