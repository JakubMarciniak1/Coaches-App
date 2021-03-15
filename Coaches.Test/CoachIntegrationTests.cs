using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coaches.CommonModels;
using Coaches.Infrastructure;
using Coaches.MainApp.Models;
using Coaches.MainApp.Repositories;
using Coaches.MainApp.Services;
using Coaches.MainApp.Services.Implementations;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Xunit;

namespace Coaches.Test
{
    public class CoachIntegrationTests
    {
        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_GetCoach_IsFound(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            var obtainedCoach = sut.GetCoach(addedCoach.Id).ResponseDTO;

            obtainedCoach.Should().Match<Coach>(c =>
                c.FirstName == firstName && c.Surname == surname &&
                c.Email == email && c.PhoneNumber == phoneNumber);
        }

        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_GetCoachList_IsFiguringOnList(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            var obtainedCoachList = sut.GetCoachList().ResponseDTO;
            var obtainedCoach = obtainedCoachList.FirstOrDefault(c => c.Id == addedCoach.Id);

            obtainedCoach.Should().NotBeNull().And.Match<Coach>(c =>
                c.FirstName == firstName && c.Surname == surname &&
                c.Email == email && c.PhoneNumber == phoneNumber);
        }


        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_UpdateCoach_IsSuccess(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            var updateResponse = sut.UpdateCoach(addedCoach);

            updateResponse.Should().Match<ServiceResponse<Coach>>(response => response.IsSuccess);

        }


        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_DeleteCoach_IsSuccess(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            var deleteResponse = sut.DeleteCoach(addedCoach.Id);

            deleteResponse.Should().Match<ServiceResponse>(response => response.IsSuccess);

        }

        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_DeleteCoach_GetCoach_IsNotFound(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            sut.DeleteCoach(addedCoach.Id);
            var getCoach = sut.GetCoach(addedCoach.Id);

            getCoach.Should().Match<ServiceResponse>(response => !response.IsSuccess && response.ErrorDetails.Code == 404);

        }

        [Theory]
        [InlineData("x", "y", "z", "5")]
        [InlineData("a", "b", "c", "%")]
        [InlineData("k", "l", null, null)]
        public void AddCoach_DeleteCoach_GetCoachList_IsNotFiguringOnList(string firstName, string surname, string email, string phoneNumber)
        {
            var trackingLogsService = new Mock<ITrackingLogsService>();
            trackingLogsService.Setup(service => service.SendEvent(
                It.IsAny<TrackingLogEvent>())).Returns(ServiceResponse.Success());

            var sut = new CoachService(new FakeCoachRepository(), trackingLogsService.Object);
            sut.EnsureInitialized("DummyURL", "0001");

            var coach = new Coach()
            {
                FirstName = firstName,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var addedCoach = sut.AddCoach(coach).ResponseDTO;
            sut.DeleteCoach(addedCoach.Id);
            var getCoachList = sut.GetCoachList().ResponseDTO;

            //getCoachList.Should().Match<List<Coach>>(response => response.Count==0);
            getCoachList.Should().NotContain(addedCoach);

        }

    }
}
