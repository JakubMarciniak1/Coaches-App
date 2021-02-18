﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using Microsoft.AspNetCore.Http;

namespace Coaches.MainApp.Services.Implementations
{
    public class CoachService : ICoachService
    {
        private readonly CoachesContext _coachesContext;
        private readonly ITrackingLogsService _trackingLogsService;

        private bool _initialized;
        private string _applicationUrl;

        public CoachService(CoachesContext coachesContext, ITrackingLogsService trackingLogsService)
        {
            _coachesContext = coachesContext;
            _trackingLogsService = trackingLogsService;

        }

        public void EnsureInitialized(string applicationUrl)
        {
            if (_initialized) return;

            _applicationUrl = applicationUrl;
            _initialized = true;
        }

        public ServiceResponse<List<Coach>> GetCoachList()
        {
            var coachesList = _coachesContext.Coach.ToList();
            _trackingLogsService.SendEvent(new TrackingLogEvent
            {
                EventDate = DateTime.UtcNow,
                EventTypeId = TrackingLogEventType.CoachListVisited,

            });
            return ServiceResponse<List<Coach>>.Success(coachesList);
        }

        public ServiceResponse<Coach> GetCoach(int id)
        {
            var coach = _coachesContext.Coach.Find(id);
            if (coach == null)
            {
                return ServiceResponse<Coach>.Error(new ErrorDetails(404, $"Could not find coach(ID {id})"));
            }
            return ServiceResponse<Coach>.Success(coach);
        }

        public ServiceResponse<Coach> AddCoach(Coach coach)
        {
            var coachEntry = _coachesContext.Coach.Add(coach);
            _coachesContext.SaveChanges();
            _trackingLogsService.SendEvent(new TrackingLogEvent
            {
                EventDate = DateTime.UtcNow,
                EventTypeId = TrackingLogEventType.CoachAdded,
                UpdatePageUrl = $"{_applicationUrl}/Coach/Update/{coach.Id}",
                CoachId = coach.Id,
            });
            return ServiceResponse<Coach>.Success(coachEntry.Entity);
        }

        public ServiceResponse<Coach> UpdateCoach(Coach coach)
        {
            var coachEntry = _coachesContext.Coach.Update(coach);
            _coachesContext.SaveChanges();
            _trackingLogsService.SendEvent(new TrackingLogEvent
            {
                EventDate = DateTime.UtcNow,
                EventTypeId = TrackingLogEventType.CoachUpdated,
                UpdatePageUrl = $"{_applicationUrl}/Coach/Update/{coach.Id}",
                CoachId = coach.Id,
            });
            return ServiceResponse<Coach>.Success(coachEntry.Entity);
        }

        public ServiceResponse DeleteCoach(int id)
        {
            var coach = _coachesContext.Coach.Find(id);
            if (coach == null)
            {
                return ServiceResponse.Error(new ErrorDetails(404, $"Could not find coach(ID {id})"));
            }
            _coachesContext.Coach.Remove(coach);
            _coachesContext.SaveChanges();
            _trackingLogsService.SendEvent(new TrackingLogEvent
            {
                EventDate = DateTime.UtcNow,
                EventTypeId = TrackingLogEventType.CoachDeleted,
                CoachId = coach.Id,
            });
            return ServiceResponse.Success();
        }
    }
}
