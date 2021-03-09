using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Coaches.MainApp.Models;
using Coaches.MainApp.Repositories;

namespace Coaches.Test
{
    class FakeCoachRepository : ICoachRepository
    {
        private static int _nextId = 1;
        Dictionary<int,Coach> _coachesDictionary = new Dictionary<int, Coach>();

        public List<Coach> GetCoaches()
        {
            return _coachesDictionary.Values.ToList();
        }

        public Coach GetCoachById(int coachId)
        {
            _coachesDictionary.TryGetValue(coachId, out var coach);
            return coach;
        }

        public Coach InsertCoach(Coach coach)
        {
            var dbCoach = new Coach
            {
                FirstName = coach.FirstName,
                Surname = coach.Surname,
                Email = coach.Email,
                PhoneNumber = coach.PhoneNumber,
                Id = _nextId
            };
            _coachesDictionary[dbCoach.Id] = dbCoach;
            _nextId++;
            return dbCoach;
        }

        public Coach UpdateCoach(Coach coach)
        {
            if(!_coachesDictionary.ContainsKey(coach.Id))
                throw new Exception("Cannot update coach. Coach doesn't exist");
            _coachesDictionary[coach.Id] = coach;
            return coach;
        }

        public void DeleteCoach(Coach coach)
        {
            if (!_coachesDictionary.ContainsKey(coach.Id))
                throw new Exception("Cannot delete coach. Coach doesn't exist");
            _coachesDictionary.Remove(coach.Id);
        }
    }
}
