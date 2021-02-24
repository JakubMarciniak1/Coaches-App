using System;
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
    public class CoachService : BaseService, ICoachService
    {
        const string Error404_CouldNotFindCoach = "Could not find coach(ID {0})";
        const string Error500_ExceptionThrownWhileSendingEvent = "Exception thrown while sending event to the Tracking app. Details:\n";
        const string Error500_FailedToSendEvent = "Failed to send event to the Tracking app. Details:\n";

        private readonly CoachesContext _coachesContext;
        private readonly ITrackingLogsService _trackingLogsService;

        private bool _initialized;
        private string _applicationUrl;
        private HttpRequest _request;

        private string UserIPAddress => _request.HttpContext.Connection.RemoteIpAddress.ToString();


        public CoachService(CoachesContext coachesContext, ITrackingLogsService trackingLogsService)
        {
            _coachesContext = coachesContext;
            _trackingLogsService = trackingLogsService;

        }

        public void EnsureInitialized(HttpRequest request)
        {
            if (_initialized) return;

            _request = request;
            _applicationUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            _initialized = true;
        }

        public ServiceResponse<List<Coach>> GetCoachList()
        {
            var response = TryExecute(() =>
            {
                var coachesList = _coachesContext.Coach.ToList();
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
                var coach = _coachesContext.Coach.Find(id);
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
                var coachEntry = _coachesContext.Coach.Add(coach);
                _coachesContext.SaveChanges();
                return ServiceResponse<Coach>.Success(coachEntry.Entity);
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
                var coachEntry = _coachesContext.Coach.Update(coach);
                _coachesContext.SaveChanges();
                return ServiceResponse<Coach>.Success(coachEntry.Entity);

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
                var coach = _coachesContext.Coach.Find(id);
                if (coach == null)
                {
                    return ServiceResponse.Error(new ErrorDetails(404, string.Format(Error404_CouldNotFindCoach, id)));
                }

                _coachesContext.Coach.Remove(coach);
                _coachesContext.SaveChanges();
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
                    UserIPAddress = UserIPAddress,
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
