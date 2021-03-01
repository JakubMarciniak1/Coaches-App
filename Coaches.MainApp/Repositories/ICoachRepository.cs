using Coaches.MainApp.Models;
using System.Collections.Generic;

namespace Coaches.MainApp.Repositories
{
    public interface ICoachRepository
    {
        List<Coach> GetCoaches();
        Coach GetCoachById(int coachId);
        Coach InsertCoach(Coach coach);
        Coach UpdateCoach(Coach coach);
        void DeleteCoach(Coach coach);
    }
}
