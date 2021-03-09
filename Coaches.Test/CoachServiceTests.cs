using System;
using System.Collections.Generic;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Data;
using Coaches.MainApp.Models;
using Coaches.MainApp.Repositories;
using Coaches.MainApp.Services;
using Coaches.MainApp.Services.Implementations;
using FluentAssertions;
using Moq;
using Xunit;

namespace Coaches.Test
{
    public class CoachServiceTests
    {
        [Fact]
        public void GetCoachList_IsSuccess()
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            coachRepositoryMock.Setup(repository => repository.GetCoaches()).Returns(new List<Coach>());
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoachList().Should().Match(response => response.As<ServiceResponse>().IsSuccess);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(100)]
        public void GetCoachList_IsCorrectCoachCount(int coachCount)
        {
            var coachList = new List<Coach>();
            for (int i = 0; i < coachCount; i++)
            {
                coachList.Add(new Coach());
            }
            var coachRepositoryMock = new Mock<ICoachRepository>();
            coachRepositoryMock.Setup(repository => repository.GetCoaches()).Returns(coachList);
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoachList().Should().BeOfType<ServiceResponse<List<Coach>>>()
                .And.Match(response => response.As<ServiceResponse<List<Coach>>>().ResponseDTO.Count == coachCount);
        }

        [Fact]
        public void GetCoach_IsSuccess()
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            coachRepositoryMock.Setup(repository => repository.GetCoachById(It.IsAny<int>())).Returns(new Coach());
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoach(1).Should().Match(response => response.As<ServiceResponse>().IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(21)]
        public void GetCoach_IsCorrectId(int coachId)
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            coachRepositoryMock.Setup(repository => repository.GetCoachById(coachId)).Returns(new Coach() { Id = coachId });
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoach(coachId).Should().BeOfType<ServiceResponse<Coach>>()
                  .And.Match(response => response.As<ServiceResponse<Coach>>().ResponseDTO.Id == coachId);
        }

        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void GetCoach_IsDataMatching(string firstName, string surname, string email, string phoneNumber)
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            coachRepositoryMock.Setup(repository => repository.GetCoachById(It.IsAny<int>()))
                .Returns(new Coach()
                {
                    FirstName = firstName,
                    Surname = surname,
                    Email = email,
                    PhoneNumber = phoneNumber
                });

            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.GetCoach(1).Should().BeOfType<ServiceResponse<Coach>>()
                .And.Match<ServiceResponse<Coach>>(response =>
                    response.ResponseDTO.FirstName == firstName && response.ResponseDTO.Surname == surname &&
                    response.ResponseDTO.Email == email && response.ResponseDTO.PhoneNumber == phoneNumber
                );
        }

        [Fact]
        public void AddCoach_IsSuccess()
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            var coach = new Coach();
            coachRepositoryMock.Setup(repository => repository.InsertCoach(coach)).Returns(coach);
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.AddCoach(coach).Should().Match<ServiceResponse<Coach>>(response => response.IsSuccess);
        }

        [Fact]
        public void AddCoach_IsNewIdReceived()
        {
            var coachRepositoryMock = new FakeCoachRepository();

            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coachResponse = sut.AddCoach(new Coach());
            var coach2Response = sut.AddCoach(new Coach());
            coach2Response.Should().Match<ServiceResponse<Coach>>(response =>
                response.ResponseDTO.Id != 0 && coachResponse.ResponseDTO.Id != 0 &&
                response.ResponseDTO.Id != coachResponse.ResponseDTO.Id);
        }

        [Fact]
        public void AddCoach_IsAdded()
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            var coach = new Coach();
            coachRepositoryMock.Setup(repository => repository.InsertCoach(coach)).Returns(coach);
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.AddCoach(coach);
            coachRepositoryMock.Verify(repository => repository.InsertCoach(coach));

        }

        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_IsDataMatching(string firstName, string surname, string email, string phoneNumber)
        {
            var coachRepositoryMock = new Mock<ICoachRepository>();
            var coach = new Coach();
            coachRepositoryMock.Setup(repository => repository.InsertCoach(coach))
                .Returns(new Coach()
                {
                    FirstName = firstName,
                    Surname = surname,
                    Email = email,
                    PhoneNumber = phoneNumber
                });


            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(coachRepositoryMock.Object, trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            sut.AddCoach(coach).Should().Match<ServiceResponse<Coach>>(response =>
                    response.ResponseDTO.FirstName == firstName && response.ResponseDTO.Surname == surname &&
                    response.ResponseDTO.Email == email && response.ResponseDTO.PhoneNumber == phoneNumber
                );
        }

    }
}
