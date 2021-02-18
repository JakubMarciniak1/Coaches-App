using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using System.Collections.Generic;

namespace Coaches.MainApp.Services
{
    public interface ICoachService
    {
        void EnsureInitialized(string applicationUrl);

        ServiceResponse<Coach> GetCoach(int id);
        ServiceResponse<Coach> AddCoach(Coach coach);
        ServiceResponse<Coach> UpdateCoach(Coach coach);
        ServiceResponse DeleteCoach(int id);
        ServiceResponse<List<Coach>> GetCoachList();
    }
}
