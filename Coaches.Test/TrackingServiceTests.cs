using System;
using System.Collections.Generic;
using System.Text;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using Coaches.MainApp.Repositories;
using Coaches.MainApp.Services;
using Coaches.MainApp.Services.Implementations;
using Coaches.Tracking.Repositories;
using Coaches.Tracking.Services.Implementations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Coaches.Test
{
    public class TrackingServiceTests
    {
        [Fact]
        public void SaveEvent_IsSuccess()
        {
            var trackingLogEventRepositoryMock = new Mock<ITrackingLogEventRepository>();
            var trackingLogEvent = new TrackingLogEvent();
            trackingLogEventRepositoryMock.Setup(repository => repository.InsertEvent(trackingLogEvent))
                .Returns(trackingLogEvent);

            var sut = new TrackingService(trackingLogEventRepositoryMock.Object);

            sut.SaveEvent(trackingLogEvent).Should().Match<ServiceResponse>(response => response.IsSuccess);
        }

        [Fact]
        public void SaveEvent_IsEventSaved()
        {
            var trackingLogEventRepositoryMock = new Mock<ITrackingLogEventRepository>();
            var trackingLogEvent = new TrackingLogEvent();
            trackingLogEventRepositoryMock.Setup(repository => repository.InsertEvent(trackingLogEvent))
                .Returns(trackingLogEvent);

            var sut = new TrackingService(trackingLogEventRepositoryMock.Object);

            sut.SaveEvent(trackingLogEvent);
            trackingLogEventRepositoryMock.Verify(repository => repository.InsertEvent(trackingLogEvent));
        }

        [Theory]
        [InlineData(null, "2017-3-1", TrackingLogEventType.CoachListVisited, "", "62.62.62.62")]
        public void SaveEvent_IsDataMatching(int? coachId, string eventDate, TrackingLogEventType eventTypeId,
            string updatePageUrl, string userIpAddress)
        {
            var trackingLogEventRepositoryMock = new Mock<ITrackingLogEventRepository>();
            var trackingLogEvent = new TrackingLogEvent()
            {
                CoachId = coachId,
                EventDate = DateTime.Parse(eventDate),
                EventTypeId = eventTypeId,
                UpdatePageUrl = updatePageUrl,
                UserIPAddress = userIpAddress,
            };
            trackingLogEventRepositoryMock.Setup(repository => repository.InsertEvent(trackingLogEvent))
                .Returns(trackingLogEvent);

            var sut = new TrackingService(trackingLogEventRepositoryMock.Object);

            sut.SaveEvent(trackingLogEvent).Should().Match<ServiceResponse<TrackingLogEvent>>(response =>
                response.ResponseDTO.CoachId == coachId &&
                response.ResponseDTO.EventDate == trackingLogEvent.EventDate &&
                response.ResponseDTO.EventTypeId == eventTypeId &&
                response.ResponseDTO.UpdatePageUrl == updatePageUrl &&
                response.ResponseDTO.UserIPAddress == userIpAddress
            );
        }


        [Fact]
        public void GetLogs_IsSuccess()
        {
            var trackingLogEventRepositoryMock = new Mock<ITrackingLogEventRepository>();
            var trackingLogEventList = new List<TrackingLogEvent>();
            trackingLogEventRepositoryMock.Setup(repository => repository.GetEvents()).Returns(trackingLogEventList);

            var sut = new TrackingService(trackingLogEventRepositoryMock.Object);

            sut.GetLogs().Should().Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.IsSuccess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(20)]
        [InlineData(21)]
        [InlineData(100)]

        // max logs stored == 20 
        public void GetLogs_IsCorrectEventsCount(int logCount)
        {
            var trackingLogEventRepositoryMock = new Mock<ITrackingLogEventRepository>();
            var trackingLogEventList = new List<TrackingLogEvent>();
            trackingLogEventRepositoryMock.Setup(repository => repository.GetEvents()).Returns(trackingLogEventList);
            for (int i = 0; i < logCount; i++)
            {
                trackingLogEventList.Add(new TrackingLogEvent());
            }
            var expectedLogCount = logCount > 20 ? 20 : logCount;

            var sut = new TrackingService(trackingLogEventRepositoryMock.Object);

            sut.GetLogs().Should().BeOfType<ServiceResponse<List<TrackingLogEvent>>>()
                    .And.Match<ServiceResponse<List<TrackingLogEvent>>>(response => response.ResponseDTO.Count == expectedLogCount);

        }
    }
}
