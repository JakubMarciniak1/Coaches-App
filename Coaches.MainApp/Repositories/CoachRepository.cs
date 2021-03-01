using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coaches.MainApp.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly CoachesContext _context;

        public CoachRepository(CoachesContext context)
        {
            _context = context;
        }

        public List<Coach> GetCoaches()
        {
            return _context.Coach.ToList();
        }

        public Coach GetCoachById(int coachId)
        {
            return _context.Coach.Find(coachId);
        }

        public Coach InsertCoach(Coach coach)
        {
            var entityEntry = _context.Coach.Add(coach);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public Coach UpdateCoach(Coach coach)
        {
            var entityEntry = _context.Coach.Update(coach);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void DeleteCoach(Coach coach)
        {
            _context.Coach.Remove(coach);
            _context.SaveChanges();
        }
    }
}
