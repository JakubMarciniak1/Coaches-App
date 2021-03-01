using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Coaches.MainApp.Repositories;

namespace Coaches.MainApp.Services.Implementations
{
    public class CoachService : BaseService, ICoachService
    {
        const string Error404_CouldNotFindCoach = "Could not find coach(ID {0})";
        const string Error500_ExceptionThrownWhileSendingEvent = "Exception thrown while sending event to the Tracking app. Details:\n";
        const string Error500_FailedToSendEvent = "Failed to send event to the Tracking app. Details:\n";

        private readonly ICoachRepository _coachRepository;
        private readonly ITrackingLogsService _trackingLogsService;

        private bool _initialized;
        private string _applicationUrl;
        private string _userIpAddress;

        public CoachService(ICoachRepository coachRepository, ITrackingLogsService trackingLogsService)
        {
            _coachRepository = coachRepository;
            _trackingLogsService = trackingLogsService;

        }

        public void EnsureInitialized(string applicationUrl, string userIpAddress)
        {
            if (_initialized) return;

            _applicationUrl = applicationUrl;
            _userIpAddress = userIpAddress;
            _initialized = true;
        }

        public ServiceResponse<List<Coach>> GetCoachList()
        {
            var response = TryExecute(() =>
            {
                var coachesList = _coachRepository.GetCoaches();
                return ServiceResponse<List<Coach>>.Success(coachesList);
            });
            if (!response.IsSuccess)
                return response;

            var sendEventResponse = SendEvent(TrackingLogEventType.CoachListVisited);
            if (!sendEventResponse.IsSuccess)
                return sendEventResponse.AsGenericResponse<List<Coach>>();

            return response;
        }

        public ServiceResponse<Coach> GetCoach(int id)
        {
            return TryExecute(() =>
            {
                var coach = _coachRepository.GetCoachById(id);
                if (coach == null)
                {
                    return ServiceResponse<Coach>.Error(new ErrorDetails(404, string.Format(Error404_CouldNotFindCoach, id)));
                }

                return ServiceResponse<Coach>.Success(coach);
            });
        }

        public ServiceResponse<Coach> AddCoach(Coach coach)
        {
            var response = TryExecute(() =>
            {
                var newCoach = _coachRepository.InsertCoach(coach);
                
                return ServiceResponse<Coach>.Success(newCoach);
            });
            if (!response.IsSuccess)
                return response;

            var sendEventResponse = SendEvent(TrackingLogEventType.CoachAdded, response.ResponseDTO.Id, true);
            if (!sendEventResponse.IsSuccess)
                return sendEventResponse.AsGenericResponse<Coach>();

            return response;
        }

        public ServiceResponse<Coach> UpdateCoach(Coach coach)
        {
            var response = TryExecute(() =>
            {
                var updatedCoach = _coachRepository.UpdateCoach(coach);
                return ServiceResponse<Coach>.Success(updatedCoach);

            });
            if (!response.IsSuccess)
                return response;

            var sendEventResponse = SendEvent(TrackingLogEventType.CoachUpdated, response.ResponseDTO.Id, true);
            if (!sendEventResponse.IsSuccess)
                return sendEventResponse.AsGenericResponse<Coach>();

            return response;
        }

        public ServiceResponse DeleteCoach(int id)
        {
            var response = TryExecute(() =>
            {
                var coach = _coachRepository.GetCoachById(id);
                if (coach == null)
                {
                    return ServiceResponse.Error(new ErrorDetails(404, string.Format(Error404_CouldNotFindCoach, id)));
                }

                _coachRepository.DeleteCoach(coach);
                return ServiceResponse.Success();
            });
            if (!response.IsSuccess)
                return response;

            var sendEventResponse = SendEvent(TrackingLogEventType.CoachDeleted, id);
            if (!sendEventResponse.IsSuccess)
                return sendEventResponse;

            return response;
        }

        private ServiceResponse SendEvent(TrackingLogEventType type, int? coachId = null, bool includeUrl = false)
        {
            try
            {
                var trackingLogEvent = new TrackingLogEvent
                {
                    EventDate = DateTime.UtcNow,
                    UserIPAddress = _userIpAddress,
                    EventTypeId = type,
                    UpdatePageUrl = includeUrl ? $"{_applicationUrl}/Coach/Update/{coachId}" : null,
                    CoachId = coachId,
                };
                var response = _trackingLogsService.SendEvent(trackingLogEvent);
                if (!response.IsSuccess)
                    return ServiceResponse.Error(new ErrorDetails(500, $"{Error500_FailedToSendEvent}{response.ErrorDetails}"));
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error(new ErrorDetails(500, $"{Error500_ExceptionThrownWhileSendingEvent}{ex}"));
            }
            return ServiceResponse.Success();
        }
    }
}
