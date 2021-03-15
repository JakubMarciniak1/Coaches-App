using System;
using System.Collections.Generic;
using System.Text;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using Coaches.MainApp.Services.Implementations;
using Coaches.Tracking.Controllers;
using Coaches.Tracking.Services.Implementations;
using FluentAssertions;
using Xunit;

namespace Coaches.Test
{
    public class GlobalIntegrationTests : IClassFixture<GlobalIntegrationFixture>
    {
        private readonly GlobalIntegrationFixture _fixture;

        public GlobalIntegrationTests(GlobalIntegrationFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void GetCoachList_GetLatestLogs_IsEventSaved()
        {
            //mocking repos
            var fakeCoachRep = new FakeCoachRepository();
            var fakeLogRep = new FakeTrackingLogEventRepository();

            //systems under test ( tracking side)
            var trackingService = new TrackingService(fakeLogRep);
            var trackingController = new TrackingController(trackingService);
            _fixture.InitTrackingController(trackingController);

            //system under test ( main app side)
            var trackingLogsService = new TrackingLogsService(_fixture.Configuration, _fixture.FakeHttpService);
            var coachService = new CoachService(fakeCoachRep, trackingLogsService);

            //testing and assertions
            var coachListResponse = coachService.GetCoachList();
            coachListResponse.Should().Match<ServiceResponse<List<Coach>>>(response => response.IsSuccess);
            var logListResponse = trackingLogsService.GetLatestLogs();
            logListResponse.Should().Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.IsSuccess);

            logListResponse.ResponseDTO.Should()
                .Contain(eventLog => eventLog.EventTypeId == TrackingLogEventType.CoachListVisited);
        }

        [Fact]
        public void AddCoach_GetLatestLogs_IsEventSaved()
        {
            //mocking repos
            var fakeCoachRep = new FakeCoachRepository();
            var fakeLogRep = new FakeTrackingLogEventRepository();

            //systems under test ( tracking side)
            var trackingService = new TrackingService(fakeLogRep);
            var trackingController = new TrackingController(trackingService);
            _fixture.InitTrackingController(trackingController);

            //system under test ( main app side)
            var trackingLogsService = new TrackingLogsService(_fixture.Configuration, _fixture.FakeHttpService);
            var coachService = new CoachService(fakeCoachRep, trackingLogsService);

            //testing and assertions
            var addCoachResponse = coachService.AddCoach(new Coach());
            addCoachResponse.Should().Match<ServiceResponse<Coach>>(response => response.IsSuccess);
            var logListResponse = trackingLogsService.GetLatestLogs(); 
            logListResponse.Should().Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.IsSuccess);

            logListResponse.ResponseDTO.Should()
                .Contain(eventLog => eventLog.EventTypeId == TrackingLogEventType.CoachAdded);
        }

        [Fact]
        public void UpdateCoach_GetLatestLogs_IsEventSaved()
        {
            //mocking repos
            var fakeCoachRep = new FakeCoachRepository();
            var newCoach = fakeCoachRep.InsertCoach(new Coach());
            var fakeLogRep = new FakeTrackingLogEventRepository();

            //systems under test ( tracking side)
            var trackingService = new TrackingService(fakeLogRep);
            var trackingController = new TrackingController(trackingService);
            _fixture.InitTrackingController(trackingController);

            //system under test ( main app side)
            var trackingLogsService = new TrackingLogsService(_fixture.Configuration, _fixture.FakeHttpService);
            var coachService = new CoachService(fakeCoachRep, trackingLogsService);

            //testing and assertions
            var updateCoachResponse = coachService.UpdateCoach(newCoach);
            updateCoachResponse.Should().Match<ServiceResponse<Coach>>(response => response.IsSuccess);
            var logListResponse = trackingLogsService.GetLatestLogs();
            logListResponse.Should().Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.IsSuccess);

            logListResponse.ResponseDTO.Should()
                .Contain(eventLog => eventLog.EventTypeId == TrackingLogEventType.CoachUpdated);
        }

        [Fact]
        public void DeleteCoach_GetLatestLogs_IsEventSaved()
        {
            //mocking repos
            var fakeCoachRep = new FakeCoachRepository();
            var newCoach = fakeCoachRep.InsertCoach(new Coach());
            var fakeLogRep = new FakeTrackingLogEventRepository();

            //systems under test ( tracking side)
            var trackingService = new TrackingService(fakeLogRep);
            var trackingController = new TrackingController(trackingService);
            _fixture.InitTrackingController(trackingController);

            //system under test ( main app side)
            var trackingLogsService = new TrackingLogsService(_fixture.Configuration, _fixture.FakeHttpService);
            var coachService = new CoachService(fakeCoachRep, trackingLogsService);

            //testing and assertions
            var deleteCoachResponse = coachService.DeleteCoach(newCoach.Id);
            deleteCoachResponse.Should().Match<ServiceResponse>(response => response.IsSuccess);
            var logListResponse = trackingLogsService.GetLatestLogs();
            logListResponse.Should().Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.IsSuccess);

            logListResponse.ResponseDTO.Should()
                .Contain(eventLog => eventLog.EventTypeId == TrackingLogEventType.CoachDeleted);
        }
    }
}
